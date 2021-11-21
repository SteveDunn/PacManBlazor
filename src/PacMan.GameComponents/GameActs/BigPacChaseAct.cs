using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Threading.Tasks;
using MediatR;
using PacMan.GameComponents.Audio;
using PacMan.GameComponents.Canvas;
using PacMan.GameComponents.Events;
using PacMan.GameComponents.Ghosts;

// ReSharper disable HeapView.ObjectAllocation.Evident

namespace PacMan.GameComponents.GameActs;

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

        _blinkyTimer = new(4500.Milliseconds(), reverseChase);

        _pacTimer = new(4750.Milliseconds(), static () => { });

        _pacMan = new() { Direction = Direction.Left };

        _bigPacMan = new(
            Vector2.Zero,
            new(31, 32),
            new(16, 16),
            new(488, 16),
            new(520, 16),
            110.Milliseconds())
        {
            Visible = false
        };

        _blinky = new(GhostNickname.Blinky, Direction.Left);

        _pacPositions = new(justOffScreen, new(-70, justOffScreen.Y));

        _blinkyPositions = new(justOffScreen + new Vector2(20, 0), new(-40, justOffScreen.Y));
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
        _blinkyTimer = new(4600.Milliseconds(), static () => { });

        _pacTimer = new(4350.Milliseconds(), async () =>
        {
            _finished = true;
            await _mediator.Publish(new CutSceneFinishedEvent());
        });

        _pacMan.Visible = false;
        _bigPacMan.Visible = true;

        var s = new LevelStats(0);
        var sess = new GhostFrightSession(s.GetLevelProps());

        _blinky.Direction = new(Direction.Right, Direction.Right);
        _blinky.SetFrightSession(sess);
        _blinky.SetFrightened();
        _blinkyPositions = _blinkyPositions.Reverse();

        var bigPacPos = _pacPositions.Reverse();
        bigPacPos = new(bigPacPos.Start - new Vector2(100, 0), bigPacPos.End);

        _pacPositions = bigPacPos;
    }
}