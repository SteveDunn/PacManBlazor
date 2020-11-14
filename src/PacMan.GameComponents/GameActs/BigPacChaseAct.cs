using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using MediatR;
using PacMan.GameComponents.Audio;
using PacMan.GameComponents.Canvas;
using PacMan.GameComponents.Events;
using PacMan.GameComponents.Ghosts;

// ReSharper disable HeapView.ObjectAllocation.Evident

namespace PacMan.GameComponents.GameActs
{
    public class BigPacChaseAct : IAct
    {
        // these need to send playerstartingevent when finished
        readonly IMediator _mediator;
        readonly IGameSoundPlayer _gameSoundPlayer;
        readonly AttractScenePacMan _pacMan;
        readonly GeneralSprite _bigPacMan;

        readonly AttractGhost _blinky;

        StartAndEndPos _pacPositions;
        EggTimer _pacTimer;

        StartAndEndPos _blinkyPositions;
        EggTimer _blinkyTimer;

        bool _finished;

        [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
        public BigPacChaseAct(IMediator mediator, IGameSoundPlayer gameSoundPlayer)
        {
            _mediator = mediator;
            _gameSoundPlayer = gameSoundPlayer;
            _finished = false;

            var justOffScreen = new Vector2(250, 140);

            _blinkyTimer = new EggTimer(4500.Milliseconds(), reverseChase);

            _pacTimer = new EggTimer(4750.Milliseconds(), () => { });

            _pacMan = new AttractScenePacMan { Direction = Directions.Left };

            _bigPacMan = new GeneralSprite(
                Vector2.Zero,
                new Size(31, 32),
                new Vector2(16, 16),
                new Vector2(488, 16),
                new Vector2(520, 16),
                110.Milliseconds())
            {
                Visible = false
            };

            _blinky = new AttractGhost(GhostNickname.Blinky, Directions.Left);

            _pacPositions = new StartAndEndPos(justOffScreen, new Vector2(-70, justOffScreen.Y));

            _blinkyPositions = new StartAndEndPos(justOffScreen + new Vector2(20, 0), new Vector2(-40, justOffScreen.Y));
        }

        public string Name { get; } = "BigPacChaseAct";

        public async ValueTask Reset()
        {
            _pacMan.Position = _pacPositions.Start;
            _blinky.Position = _blinkyPositions.Start;

            await _gameSoundPlayer.CutScene();
        }

        public async ValueTask<ActUpdateResult> Update(CanvasTimingInformation timing)
        {
            _blinkyTimer.Run(timing);
            _pacTimer.Run(timing);
            await _bigPacMan.Update(timing);

            lerpBlinky();
            lerpPacMan();

            await _pacMan.Update(timing);
            await _blinky.Update(timing);
            await _bigPacMan.Update(timing);

            return _finished ? ActUpdateResult.Finished : ActUpdateResult.Running;
        }

        public async ValueTask Draw(CanvasWrapper canvas)
        {
            await _blinky.Draw(canvas);
            await _pacMan.Draw(canvas);
            await _bigPacMan.Draw(canvas);
        }

        void lerpBlinky()
        {
            float pc = _blinkyTimer.Progress;

            _blinky.Position = Vector2.Lerp(_blinkyPositions.Start, _blinkyPositions.End, pc);
        }

        void lerpPacMan()
        {
            float pc = _pacTimer.Progress;

            _pacMan.Position = Vector2.Lerp(_pacPositions.Start, _pacPositions.End, pc);
            _bigPacMan.Position = Vector2.Lerp(_pacPositions.Start, _pacPositions.End, pc);
        }

        void reverseChase()
        {
            _blinkyTimer = new EggTimer(4600.Milliseconds(), () => { });

            _pacTimer = new EggTimer(4350.Milliseconds(), async () =>
            {
                _finished = true;
                await _mediator.Publish(new CutSceneFinishedEvent());
            });

            _pacMan.Visible = false;
            _bigPacMan.Visible = true;

            var s = new LevelStats(0);
            var sess = new GhostFrightSession(s.GetLevelProps());

            _blinky.Direction = new DirectionInfo(Directions.Right, Directions.Right);
            _blinky.SetFrightSession(sess);
            _blinky.SetFrightened();
            _blinkyPositions = _blinkyPositions.Reverse();

            var bigPacPos = _pacPositions.Reverse();
            bigPacPos = new StartAndEndPos(bigPacPos.Start - new Vector2(100, 0), bigPacPos.End);

            _pacPositions = bigPacPos;
        }
    }
}