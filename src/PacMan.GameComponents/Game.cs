using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Threading.Tasks;
using Blazor.Extensions.Canvas.Canvas2D;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PacMan.GameComponents.Audio;
using PacMan.GameComponents.Canvas;
using PacMan.GameComponents.Events;
using PacMan.GameComponents.GameActs;
using PacMan.GameComponents.Ghosts;
using PacMan.GameComponents.Requests;

namespace PacMan.GameComponents
{
    public class Game : IGame
    {
        CanvasTimingInformation? _canvasTimingInformation;
        // ReSharper disable once UnusedMember.Local
        readonly DiagPanel _diagPanel = new DiagPanel();

        readonly IGameSoundPlayer _gameSoundPlayer;
        readonly IHumanInterfaceParser _input;
        readonly IPacMan _pacman;

        readonly IScorePanel _scorePanel;
        readonly IMediator _mediator;
        readonly IFruit _fruit;
        readonly IStatusPanel _statusPanel;

        // ReSharper disable once UnusedMember.Local

        IAct? _currentAct;
        TimedSpriteList? _tempSprites;
        EggTimer _pauser = EggTimer.Unset;
        
        CanvasWrapper? _scoreCanvas;
        CanvasWrapper? _mazeCanvas;
        CanvasWrapper? _statusCanvas;
        // ReSharper disable once NotAccessedField.Local
        CanvasWrapper? _diagCanvas;

        static bool _initialised;

        public Game(
            IMediator mediator,
            IFruit fruit,
            IStatusPanel statusPanel,
            IScorePanel scorePanel,
            IGameSoundPlayer gameSoundPlayer,
            IHumanInterfaceParser input,
            IPacMan pacman)
        {
            _mediator = mediator;
            _fruit = fruit;
            _statusPanel = statusPanel;
            _scorePanel = scorePanel;
            _gameSoundPlayer = gameSoundPlayer;
            _input = input;
            _pacman = pacman;

        }


        [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
        public async ValueTask Initialise(IJSRuntime jsRuntime)
        {
            _canvasTimingInformation = new CanvasTimingInformation();

            _tempSprites = new TimedSpriteList();

            _pauser = new EggTimer(0.Milliseconds(), () => { });


            // POINTER: You can change the starting Act by using something like:
            //_currentAct = new TornGhostChaseAct(new AttractAct());
            
            // ReSharper disable once HeapView.BoxingAllocation
            _currentAct = await _mediator.Send(new GetActRequest("AttractAct"));

            await _gameSoundPlayer.LoadAll(jsRuntime);

            _initialised = true;
        }

        async ValueTask update()
        {
            ensureInitialised();
            _input.Update(_canvasTimingInformation!);

            _scorePanel.Update(_canvasTimingInformation!);
            _statusPanel.Update(_canvasTimingInformation!);

            _tempSprites!.Update(_canvasTimingInformation!);

            _pauser.Run(_canvasTimingInformation!);

            await checkCheatKeys();

            if (_pauser.Finished)
            {
                await _currentAct!.Update(_canvasTimingInformation!);
            }
        }

        async ValueTask checkCheatKeys()
        {
            if (Cheats.AllowDebugKeys && _input.IsKeyCurrentlyDown(Keys.Three))
            {
                // ReSharper disable once HeapView.BoxingAllocation
                _currentAct = await _mediator.Send(new GetActRequest("LevelFinishedAct"));

            }
            if (Cheats.AllowDebugKeys && _input.IsKeyCurrentlyDown(Keys.Four))
            {
                await _mediator.Publish(new PacManEatenEvent());
            }

            if (Cheats.AllowDebugKeys && _input.WasKeyPressedAndReleased(Keys.Six))
            {
                await _mediator.Publish(new AllPillsEatenEvent());
            }

        }

        async ValueTask draw()
        {
            ensureInitialised();
            
            var dim = Constants.UnscaledCanvasSize;

            if (_underlyingCanvasContext == null)
            {
                // ReSharper disable once HeapView.ObjectAllocation.Evident
                throw new InvalidOperationException($"{nameof(SetCanvasContextForOutput)} has not been called!");
            }

            await _underlyingCanvasContext.BeginBatchAsync();
            
            await _mazeCanvas!.Clear((int) dim.X,(int) dim.Y);

            await _scorePanel.Draw(_scoreCanvas!);
            
            await _statusPanel.Draw(_statusCanvas!);
            await _currentAct!.Draw(_mazeCanvas);
            await _tempSprites!.Draw(_mazeCanvas);

            if (DiagInfo.ShouldShow)
            {
                await _diagPanel.Draw(_diagCanvas!);
            }

            await _underlyingCanvasContext.EndBatchAsync();
        }

        static void ensureInitialised()
        {
            if (!_initialised)
            {
                throw new InvalidOperationException("Not initialised!");
            }
        }

        public ValueTask FruitEaten(int points)
        {
            ensureInitialised();
            _tempSprites!.Add(new TimedSprite(3000, new ScoreSprite(_fruit.Position, points)));
            
            return default;
        }

        public ValueTask GhostEaten(IGhost ghost, int points)
        {
            ensureInitialised();
            _tempSprites!.Add(new TimedSprite(900, new ScoreSprite(_pacman.Position, points)));

            ghost.Visible = false;
            _pacman.Visible = false;

            _pauser = new EggTimer(1000.Milliseconds(), () =>
            {
                ghost.Visible = true;
                _pacman.Visible = true;
            });

            return default;
        }

        public void SetCanvasContextForOutput(Canvas2DContext context)
        {
            _underlyingCanvasContext = context ?? throw new ArgumentNullException(nameof(context));
            
            _scoreCanvas = new CanvasWrapper(context, new Point(0, 0));
            _mazeCanvas = new CanvasWrapper(context, new Point(0, 26));
            _diagCanvas = new CanvasWrapper(context, new Point(0, 220));
            _statusCanvas = new CanvasWrapper(context, new Point(0, 274));
        }

        public void SetCanvasesForPlayerMazes(Canvas2DContext player1MazeCanvas, Canvas2DContext player2MazeCanvas)
        {
            _ = player1MazeCanvas ?? throw new InvalidOperationException("null canvas!");
            _ = player2MazeCanvas ?? throw new InvalidOperationException("null canvas!");

            MazeCanvases.Populate(new MazeCanvas(player1MazeCanvas), new MazeCanvas(player2MazeCanvas));
        }

        static readonly Stopwatch _stopWatch = new Stopwatch();

        static float getTimestep() => 1000 / (float)Constants.FramesPerSecond;

        static float _delta;
        
        float _lastTimestamp;
        Canvas2DContext? _underlyingCanvasContext;

        static int _frameCount;
        bool _postRenderInitialised;

        public async ValueTask RunGameLoop(float timestamp)
        {
            _stopWatch.Restart();
            
            if (!_initialised || !_postRenderInitialised)
            {
                return;
            }

            _delta += timestamp - _lastTimestamp;
            _lastTimestamp = timestamp;

            var timestep = getTimestep();
            while (_delta >= timestep)
            {
                _canvasTimingInformation!.Update(timestep);

                DiagInfo.IncrementUpdateCount();
                await DiagInfo.Update(_canvasTimingInformation, _input);
                await update();

                _delta -= timestep;
                if (Debugger.IsAttached)
                {
                    _delta = timestep;
                }
            }

            await draw();

            DiagInfo.IncrementDrawCount(timestamp);

            ++_frameCount;

            // ReSharper disable once UnusedVariable
            int fps = (int) (_canvasTimingInformation!.TotalTime.TotalMilliseconds / _frameCount);

            long elapsedms = _stopWatch.ElapsedMilliseconds;
            
            //Console.WriteLine($"** game loop took {elapsedms} ms - {fps} FPS");

            DiagInfo.UpdateTimeLoopTaken(elapsedms);

         //   Debug.WriteLine($"slow:{DiagInfo.SlowElapsedCount:D} GameLoopDurationMs:{DiagInfo.GameLoopDurationMs:D} max dur:{DiagInfo.MaxGameLoopDurationMs:D}");
        }


        public void PostRenderInitialize(Canvas2DContext outputCanvasContext,
            Canvas2DContext player1MazeCanvas,
            Canvas2DContext player2MazeCanvas,
            in ElementReference spritesheetReference)
        {

            SetCanvasContextForOutput(outputCanvasContext);
            SetCanvasesForPlayerMazes(player1MazeCanvas, player2MazeCanvas);

            Spritesheet.SetReference(spritesheetReference);

            _postRenderInitialised = true;
        }

        public void SetAct(IAct act)
        {
            _currentAct = act ?? throw new ArgumentNullException(nameof(act));
        }
    }
}