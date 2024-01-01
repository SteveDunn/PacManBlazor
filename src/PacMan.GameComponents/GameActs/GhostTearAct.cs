using System.Drawing;
using PacMan.GameComponents.Ghosts;

namespace PacMan.GameComponents.GameActs;

public class GhostTearAct : IAct
{
    private readonly IGameSoundPlayer _gameSoundPlayer;
    private readonly IMediator _mediator;

    private enum Stage
    {
        MovingBlinky,
        TearingBlinky,
        BlinkyLooking
    }

    private GeneralSprite _snagSprite;
    private Stage _stage;
    private bool _finished;
    private int _animFrame;
    private GeneralSprite _lookingBlinky;
    private LoopingTimer _lookTimer;

    private readonly AttractScenePacMan _pacMan;

    private readonly AttractGhost _blinky;

    private readonly StartAndEndPos _pacPositions;
    private readonly EggTimer _pacTimer;
    private readonly LoopingTimer _tearTimer;

    private readonly StartAndEndPos _blinkyPositions;
    private readonly EggTimer _blinkyTimer;

    private readonly Vector2 _centerPoint = new(120, 140);

    private readonly Vector2[] _tearFrames;
    private readonly Vector2[] _blinkyLookFrames;

    private readonly Size _tearSize = new(13, 13);
    private readonly Vector2 _tearOffset = new(7, 6.5f);

    public GhostTearAct(IGameSoundPlayer gameSoundPlayer, IMediator mediator)
    {
        _gameSoundPlayer = gameSoundPlayer;
        _mediator = mediator;

        _finished = false;

        _stage = Stage.MovingBlinky;
        _animFrame = 0;

        _tearFrames =
        [
            new Vector2(589, 98),
            new Vector2(609, 98),
            new Vector2(622, 98),
            new Vector2(636, 98),
            new Vector2(636, 98),
            new Vector2(636, 98),
            new Vector2(649, 98)
        ];

        _blinkyLookFrames =
        [
            new Vector2(584, 113),
            new Vector2(600, 113)
        ];

        _blinkyTimer = new(4500.Milliseconds(), BlinkyCaught);

        _tearTimer = new(
            500.Milliseconds(),
            () => {
                if (_stage == Stage.TearingBlinky)
                {
                    UpdateTearAnimation();
                }
            });

        _lookTimer = new(TimeSpan.MaxValue, () => { });

        _pacTimer = new(4750.Milliseconds(), () => { });

        _pacMan = new() {
            Direction = Direction.Left
        };

        _snagSprite = new(_centerPoint, _tearSize, _tearOffset, _tearFrames[0])
        {
            Visible = true
        };

        _blinky = new(GhostNickname.Blinky, Direction.Left);
        _lookingBlinky = new NullSprite();

        var justOffScreen = new Vector2(250, 140);

        _pacPositions = new(justOffScreen, new(-70, justOffScreen.Y));

        _blinkyPositions = new(justOffScreen + new Vector2(120, 0), _centerPoint - new Vector2(10, 0));
    }

    public async ValueTask Reset()
    {
        await _gameSoundPlayer.CutScene();
        _pacMan.Position = _pacPositions.Start;
        _blinky.Position = _blinkyPositions.Start;
    }

    public string Name => "GhostTearAct";

    public async ValueTask<ActUpdateResult> Update(CanvasTimingInformation timing)
    {
        _blinkyTimer.Run(timing);
        _pacTimer.Run(timing);
        _tearTimer.Run(timing);
        _lookTimer.Run(timing);

        if (_stage == Stage.MovingBlinky)
        {
            LerpBlinky();
        }

        LerpPacMan();

        await _pacMan.Update(timing);
        await _blinky.Update(timing);
        await _lookingBlinky.Update(timing);
        await _snagSprite.Update(timing);

        return _finished ? ActUpdateResult.Finished : ActUpdateResult.Running;
    }

    public async ValueTask Draw(CanvasWrapper canvas)
    {
        await _pacMan.Draw(canvas);
        await _blinky.Draw(canvas);
        await _lookingBlinky.Draw(canvas);
        await _snagSprite.Draw(canvas);
    }

    private void LerpBlinky()
    {
        var pc = _blinkyTimer.Progress;
        _blinky.Position = Vector2.Lerp(_blinkyPositions.Start, _blinkyPositions.End, pc);
    }

    private void LerpPacMan()
    {
        var pc = _pacTimer.Progress;

        _pacMan.Position = Vector2.Lerp(_pacPositions.Start, _pacPositions.End, pc);
    }

    private void BlinkyCaught() => _stage = Stage.TearingBlinky;

    private void UpdateTearAnimation()
    {
        ++_animFrame;
        if (_animFrame < _tearFrames.Length)
        {
            _snagSprite = new(
                _centerPoint,
                _tearSize,
                _tearOffset,
                _tearFrames[_animFrame]);

            // this.snagSprite.position = this.snagSprite.position.minus(new Point(1, 0));

            _blinky.Position -= new Vector2(1, 0);

            return;
        }

        _animFrame = 0;
        _blinky.Visible = false;
        SetLookingBlinky(0);

        _lookTimer = new(1500.Milliseconds(), async () => await UpdateBlinkyLookAnimation());

        _lookingBlinky.Visible = true;
        _stage = Stage.BlinkyLooking;
    }

    private void SetLookingBlinky(int frame) =>
        _lookingBlinky = new(
            _blinky.Position,
            _blinky.Size,
            _blinky.Origin,
            _blinkyLookFrames[frame])
        {
            Visible = true
        };

    private async ValueTask UpdateBlinkyLookAnimation()
    {
        ++_animFrame;

        if (_animFrame == 2)
        {
            _animFrame = 0;
            await _mediator.Publish(new CutSceneFinishedEvent());

            _finished = true;
            return;
        }

        SetLookingBlinky(_animFrame);
    }
}