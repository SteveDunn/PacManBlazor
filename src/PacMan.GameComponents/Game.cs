using System.Diagnostics;
using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PacMan.GameComponents.Ghosts;
// ReSharper disable NullableWarningSuppressionIsUsed

namespace PacMan.GameComponents;

public class Game : IGame
{
    // ReSharper disable once UnusedMember.Local
    readonly DiagPanel _diagPanel = new();

    readonly IGameSoundPlayer _gameSoundPlayer;
    readonly IHumanInterfaceParser _input;
    readonly IPacMan _pacman;

    readonly IScorePanel _scorePanel;
    readonly IMediator _mediator;
    readonly IFruit _fruit;
    readonly IStatusPanel _statusPanel;

    CanvasTimingInformation? _canvasTimingInformation;

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
        _canvasTimingInformation = new();

        _tempSprites = new();

        _pauser = new(0.Milliseconds(), () => { });

        // POINTER: You can change the starting Act by using something like:
        // _currentAct = new TornGhostChaseAct(new AttractAct());

        // ReSharper disable once HeapView.BoxingAllocation
        _currentAct = await _mediator.Send(new GetActRequest("AttractAct"));

        await _gameSoundPlayer.LoadAll(jsRuntime);

        _initialised = true;
    }

    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    async ValueTask Update()
    {
        EnsureInitialised();
        _input.Update(_canvasTimingInformation!);

        _scorePanel.Update(_canvasTimingInformation!);
        _statusPanel.Update(_canvasTimingInformation!);

        _tempSprites!.Update(_canvasTimingInformation!);

        _pauser.Run(_canvasTimingInformation!);

        await CheckCheatKeys();

        if (_pauser.Finished)
        {
            await _currentAct!.Update(_canvasTimingInformation!);
        }
    }

    async ValueTask CheckCheatKeys()
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

    async ValueTask Draw()
    {
        EnsureInitialised();

        var dim = Constants.UnscaledCanvasSize;

        if (_underlyingCanvasContext == null)
        {
            // ReSharper disable once HeapView.ObjectAllocation.Evident
            throw new InvalidOperationException($"{nameof(SetCanvasContextForOutput)} has not been called!");
        }

        await _underlyingCanvasContext.BeginBatchAsync();

        await _mazeCanvas!.Clear((int) dim.X, (int) dim.Y);

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

    static void EnsureInitialised()
    {
        if (!_initialised)
        {
            throw new InvalidOperationException("Not initialised!");
        }
    }

    public ValueTask FruitEaten(Points points)
    {
        EnsureInitialised();
        _tempSprites!.Add(new(3000, new ScoreSprite(_fruit.Position, points)));

        return default;
    }

    public ValueTask GhostEaten(IGhost ghost, Points points)
    {
        EnsureInitialised();
        _tempSprites!.Add(new(900, new ScoreSprite(_pacman.Position, points)));

        ghost.Visible = false;
        _pacman.Visible = false;

        _pauser = new(1000.Milliseconds(), () => {
            ghost.Visible = true;
            _pacman.Visible = true;
        });

        return default;
    }

    public void SetCanvasContextForOutput(Canvas2DContext context)
    {
        _underlyingCanvasContext = context ?? throw new ArgumentNullException(nameof(context));

        _scoreCanvas = new(context, new(0, 0));
        _mazeCanvas = new(context, new(0, 26));
        _diagCanvas = new(context, new(0, 220));
        _statusCanvas = new(context, new(0, 274));
    }

    public void SetCanvasesForPlayerMazes(Canvas2DContext player1MazeCanvas, Canvas2DContext player2MazeCanvas)
    {
        _ = player1MazeCanvas ?? throw new InvalidOperationException("null canvas!");
        _ = player2MazeCanvas ?? throw new InvalidOperationException("null canvas!");

        MazeCanvases.Populate(new(player1MazeCanvas), new(player2MazeCanvas));
    }

    static readonly Stopwatch _stopWatch = new();

    static float GetTimestep() => 1000 / (float) Constants.FramesPerSecond;

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

        // timestep would normally be fixed (at 60FPS), but we have
        // the ability to slow down and speed up the game (via the A and S keys)
        var timestep = GetTimestep();

        while (_delta >= timestep)
        {
            Pnrg.Update();
            _canvasTimingInformation!.Update(timestep);

            DiagInfo.IncrementUpdateCount();
            await DiagInfo.Update(_canvasTimingInformation, _input);
            await Update();

            _delta -= timestep;

            // if (Debugger.IsAttached)
            // {
            //     _delta = 0;
            // }
        }

        await Draw();

        DiagInfo.IncrementDrawCount(timestamp);

        ++_frameCount;

        var fps = (int) (_canvasTimingInformation!.TotalTime.TotalMilliseconds / _frameCount);
        DiagInfo.Fps = fps;

        DiagInfo.UpdateTimeLoopTaken(_stopWatch.ElapsedMilliseconds);
    }

    public void PostRenderInitialize(
        Canvas2DContext outputCanvasContext,
        Canvas2DContext player1MazeCanvas,
        Canvas2DContext player2MazeCanvas,
        in ElementReference spritesheetReference)
    {
        SetCanvasContextForOutput(outputCanvasContext);
        SetCanvasesForPlayerMazes(player1MazeCanvas, player2MazeCanvas);

        Spritesheet.SetReference(spritesheetReference);

        _postRenderInitialised = true;
    }

    public void SetAct(IAct act) => _currentAct = act ?? throw new ArgumentNullException(nameof(act));
}