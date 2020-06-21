using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MediatR;
using PacMan.GameComponents.Canvas;
using PacMan.GameComponents.Events;

namespace PacMan.GameComponents.GameActs
{
    /// When the level is finished, the screen flashes white and blue.
    /// Transitions into either the cut-scene act if a 'cut-scene' is due, or the 'player intro' act.
    public class LevelFinishedAct : IAct
    {
        readonly IMediator _mediator;
        readonly IGhostCollection _ghostCollection;
        readonly IPacMan _pacman;
        readonly IMaze _maze;
        int _step;
        LoopingTimer _timer = LoopingTimer.DoNothing;
        bool _finished;

        public LevelFinishedAct(IMediator mediator, IGhostCollection ghostCollection, IPacMan pacman, IMaze maze)
        {
            _mediator = mediator;
            _ghostCollection = ghostCollection;
            _pacman = pacman;
            _maze = maze;
            _step = 0;
        }

        public string Name => "LevelFinishedAct";

        [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
        public ValueTask Reset()
        {
            _finished = false;
            
            _timer = new LoopingTimer(2.Seconds(), () =>
            {
                _step += 1;
                _maze.StartFlashing();

                _ghostCollection.Ghosts.ForEach(g => g.Visible = false);

                _timer = new LoopingTimer(2.Seconds(), async () =>
                {
                    _step += 1;
                    _maze.StopFlashing();

                    await _mediator.Publish(new LevelFinishedEvent());

                    _finished = true;
                });
            });

            return default;
        }

        public async ValueTask<ActUpdateResult> Update(CanvasTimingInformation timing)
        {
            if (_finished)
            {
                return ActUpdateResult.Finished;
            }
            _timer.Run(timing);

            await _maze.Update(timing);
            await _pacman.Update(timing);

            return ActUpdateResult.Running;
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
}