using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MediatR;
using PacMan.GameComponents.Canvas;
using PacMan.GameComponents.Requests;

namespace PacMan.GameComponents.GameActs;

/// <summary>
/// Introduces the current player, shows player X and ready for 3 seconds, then ghosts and ready for 3 seconds.
/// Transitions to either the 'demo act' (if in demo mode), otherwise the 'game act'
/// </summary>
public class DemoPlayerIntroAct : IAct
{
    readonly IMediator _mediator;
    readonly IGame _game;
    readonly IGhostCollection _ghostCollection;
    readonly IPacMan _pacman;
    readonly IMaze _maze;
    int _progress;
    bool _finished;

    LoopingTimer _currentTimer = LoopingTimer.DoNothing;

    public DemoPlayerIntroAct(
        IMediator mediator,
        IGame game,
        IGhostCollection ghostCollection,
        IPacMan pacman,
        IMaze maze)
    {
        _mediator = mediator;
        _game = game;
        _ghostCollection = ghostCollection;
        _pacman = pacman;
        _maze = maze;
    }

    public string Name => "DemoPlayerIntroAct";

    [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
    public ValueTask Reset()
    {
        _progress = 0;

        var timeToShowPlayerNumberAndHideGhosts = 0.Seconds();

        _currentTimer = new(timeToShowPlayerNumberAndHideGhosts, () =>
        {
            _progress += 1;

            _currentTimer = new(2.Seconds(), () => _finished = true);
        });

        return default;
    }

    public async ValueTask<ActUpdateResult> Update(CanvasTimingInformation timing)
    {
        _currentTimer.Run(timing);

        if (_finished)
        {
            // ReSharper disable once HeapView.BoxingAllocation
            var act = await _mediator.Send(new GetActRequest("GameAct"));

            _game.SetAct(act);
        }

        return ActUpdateResult.Finished;
    }

    public async ValueTask Draw(CanvasWrapper session)
    {
        await _maze.Draw(session);

        await session.DrawMyText("GAME OVER", TextPoints.GameOverPoint, Colors.Red);

        if (_progress == 1)
        {
            await _pacman.Draw(session);
            await _ghostCollection.DrawAll(session);
        }
    }
}