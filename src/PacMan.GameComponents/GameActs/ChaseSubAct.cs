using System.Drawing;
using PacMan.GameComponents.Ghosts;

namespace PacMan.GameComponents.GameActs;

public class ChaseSubAct
{
    readonly TimerList _tempTimers;

    readonly List<AttractGhost> _ghosts;

    readonly AttractScenePacMan _pacMan;

    readonly PowerPill _powerPillToEat;
    readonly PowerPill _powerPillLegend;
    readonly Pill _pillLegend;

    readonly Dictionary<GhostNickname, StartAndEndPos> _ghostPositions;
    readonly TimedSpriteList _tempSprites;

    bool _legendVisible;
    bool _copyrightVisible;
    bool _ghostsChasing;

    Points _ghostPoints;
    StartAndEndPos _pacPositions;

    EggTimer _ghostTimer;
    EggTimer _pacTimer;

    EggTimer _ghostEatenTimer;

    bool _finished;

    [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
    public ChaseSubAct()
    {
        _finished = false;
        _tempTimers = new();

        _ghostPoints = Points.From(200);

        _tempSprites = new();
        _ghosts = new();

        var justOffScreen = new Vector2(250, 140);

        _ghostEatenTimer = new(0.Milliseconds(), static () => { });
        _ghostTimer = new(5.Seconds(), async () => await reverseChase());
        _pacTimer = new(5.1f.Seconds(), static () => { });

        _powerPillToEat = new() {
            Visible = false,
            Position = new(30, justOffScreen.Y - 4)
        };

        _pillLegend = new() { Position = new(70, 178) };

        _powerPillLegend = new() { Position = new(66, 182) };

        _pacMan = new() { Direction = Direction.Left };

        var blinky = new AttractGhost(GhostNickname.Blinky, Direction.Left);
        var pinky = new AttractGhost(GhostNickname.Pinky, Direction.Left);
        var inky = new AttractGhost(GhostNickname.Inky, Direction.Left);
        var clyde = new AttractGhost(GhostNickname.Clyde, Direction.Left);

        _ghosts.Add(blinky);
        _ghosts.Add(pinky);
        _ghosts.Add(inky);
        _ghosts.Add(clyde);

        var gap = new Vector2(16, 0);

        _pacPositions = new(justOffScreen, new(30, justOffScreen.Y));
        _pacMan.Position = _pacPositions.Start;

        _ghostPositions = new();

        var startPos = justOffScreen + new Vector2(50, 0);
        var endPos = new Vector2(50, justOffScreen.Y);

        blinky.Position = startPos;
        _ghostPositions[blinky.NickName] = new(startPos, endPos);

        startPos += gap;
        endPos += gap;
        pinky.Position = startPos;
        _ghostPositions[pinky.NickName] = new(startPos, endPos);

        startPos += gap;
        endPos += gap;
        inky.Position = startPos;
        _ghostPositions[inky.NickName] = new(startPos, endPos);

        startPos += gap;
        endPos += gap;
        clyde.Position = startPos;
        _ghostPositions[clyde.NickName] = new(startPos, endPos);

        _tempTimers.Add(new(1.Seconds(), () => _legendVisible = true));
        _tempTimers.Add(new(3.Seconds(), () =>
        {
            _powerPillToEat.Visible = true;
            _copyrightVisible = true;
        }));

        _tempTimers.Add(new(4500.Milliseconds(), () =>
        {
            _ghostsChasing = true;
        }));
    }

    public async ValueTask<ActUpdateResult> Update(CanvasTimingInformation timing)
    {
        if (_finished)
        {
            return ActUpdateResult.Finished;
        }

        _tempTimers.Update(timing);

        if (_ghostsChasing)
        {
            await _powerPillLegend.Update(timing);
            await _powerPillToEat.Update(timing);
            _ghostTimer.Run(timing);
            _pacTimer.Run(timing);
            _ghostEatenTimer.Run(timing);
        }

        await _pillLegend.Update(timing);

        _tempSprites.Update(timing);

        lerpPacMan();

        foreach (var g in _ghosts)
        {
            if (!g.Alive)
            {
                continue;
            }

            lerpGhost(g);

            await g.Update(timing);

            if (Vector2s.AreNear(_pacMan.Position, g.Position, 2))
            {
                ghostEaten(g);

                if (g.NickName == GhostNickname.Clyde)
                {
                    _tempTimers.Add(new(1.Seconds(), () =>
                    {
                        _finished = true;
                    }));
                }
            }
        }

        await _pacMan.Update(timing);

        return ActUpdateResult.Running;
    }

    public async ValueTask Draw(CanvasWrapper session)
    {
        await _powerPillToEat.Draw(session);

        if (_legendVisible)
        {
            await _powerPillLegend.Draw(session);
            await _pillLegend.Draw(session);

            await session.DrawMyText("10 pts", new(80, 170), Colors.White);
            await session.DrawMyText("50 pts", new(80, 180), Colors.White);
        }

        if (_copyrightVisible)
        {
            await session.DrawMyText("© 1980 midway mfg. co.", new(10, 220), Colors.White);
        }

        await _tempSprites.Draw(session);

        foreach (var eachGhost in _ghosts)
        {
            if (eachGhost.Alive)
            {
                await eachGhost.Draw(session);
            }
        }

        await _pacMan.Draw(session);

        var gp = _ghosts[0].Position;

        await session.DrawText("STEVE DUNN 2022", new((int)gp.X + 2, (int)(gp.Y + 22)), Color.Black);
        await session.DrawText("STEVE DUNN 2022", new((int)gp.X, (int)(gp.Y + 20)), Color.Yellow);
    }

    void ghostEaten(AttractGhost ghost)
    {
        ghost.Alive = false;

        _pacMan.Visible = false;

        _ghostTimer.Pause();
        _pacTimer.Pause();

        showScore(ghost.Position, _ghostPoints);

        _ghostPoints = Points.From(_ghostPoints.Value * 2);

        // ReSharper disable once HeapView.ObjectAllocation.Evident
        _ghostEatenTimer = new(1.Seconds(), () =>
        {
            _ghostTimer.Resume();
            _pacTimer.Resume();
            _pacMan.Visible = true;
        });
    }

    void showScore(Vector2 pos, Points amount) =>
        _tempSprites.Add(new(900, new ScoreSprite(pos, amount)));

    void lerpGhost(AttractGhost ghost)
    {
        StartAndEndPos positions = _ghostPositions[ghost.NickName];

        ghost.Position = Vector2s.Lerp(positions.Start, positions.End, _ghostTimer.Progress);
    }

    void lerpPacMan()
    {
        _pacMan.Position = Vector2s.Lerp(_pacPositions.Start, _pacPositions.End, _pacTimer.Progress);
    }

    [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
    ValueTask reverseChase()
    {
        _powerPillToEat.Visible = false;
        _ghostTimer = new(12.Seconds(), () => { });
        _pacTimer = new(6.Seconds(), () => { });

        var levelStats = new LevelStats(0);
        var frightSessions = new GhostFrightSession(levelStats.GetLevelProps());

        foreach (var eachGhost in _ghosts)
        {
            eachGhost.Direction.Update(Direction.Right);
            eachGhost.SetFrightSession(frightSessions);
            eachGhost.SetFrightened();

            _ghostPositions[eachGhost.NickName] = _ghostPositions[eachGhost.NickName].Reverse();
        }

        _pacPositions = _pacPositions.Reverse();

        _pacMan.Direction = Direction.Right;

        return default;
    }
}