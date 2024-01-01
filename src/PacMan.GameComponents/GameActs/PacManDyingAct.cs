namespace PacMan.GameComponents.GameActs;

/// An act that shows Pacman dying.  Transitions to either: the 'attract act' (if in demo mode), the 'game over act' if all players are dead,
/// or the 'attract act' for the next player that's alive.
public class PacManDyingAct : IAct
{
    private readonly IMediator _mediator;
    private readonly IGhostCollection _ghostCollection;
    private readonly IGameSoundPlayer _gameSoundPlayer;
    private readonly IPacMan _pacman;
    private readonly IMaze _maze;

    private int _step;
    private LoopingTimer _timer = LoopingTimer.DoNothing;
    private bool _finished;

    public PacManDyingAct(IMediator mediator, IGhostCollection ghostCollection, IGameSoundPlayer gameSoundPlayer, IPacMan pacman, IMaze maze)
    {
        _mediator = mediator;
        _ghostCollection = ghostCollection;
        _gameSoundPlayer = gameSoundPlayer;
        _pacman = pacman;
        _maze = maze;
    }

    public string Name => "PacManDyingAct";

    public async ValueTask Reset()
    {
        await _gameSoundPlayer.Reset();

        _step = 0;
        _finished = false;

        _pacman.StartDigesting();
        _ghostCollection.Ghosts.ForEach(g => g.StopMoving());

        _timer = new(2.Seconds(), () =>
        {
            _step += 1;
            _ = _gameSoundPlayer.PacManDying();

            _pacman.StartDying();

            _timer = new(2.Seconds(), async () =>
            {
                _step += 1;
                _finished = true;
                await _mediator.Publish(new PacManDeadEvent());
            });
        });
    }

    public async ValueTask<ActUpdateResult> Update(CanvasTimingInformation timing)
    {
        if (_finished)
        {
            return ActUpdateResult.Finished;
        }

        _timer.Run(timing);

        await _pacman.Update(timing);

        await _ghostCollection.Update(timing);

        return _finished ? ActUpdateResult.Finished : ActUpdateResult.Running;
    }

    public async ValueTask Draw(CanvasWrapper session)
    {
        await _maze.Draw(session);
        await _pacman.Draw(session);

        if (_step == 0)
        {
            await _ghostCollection.DrawAll(session);
        }
    }
}