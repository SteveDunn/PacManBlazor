using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PacMan.GameComponents.Audio;
using PacMan.GameComponents.Ghosts;
using PacMan.GameComponents.Requests;

namespace PacMan.GameComponents.Events
{
    public readonly struct PowerPillEatenEvent : INotification
    {
        public PowerPillEatenEvent(CellIndex cellIndex) => CellIndex = cellIndex;

        public CellIndex CellIndex { get; }

        public class Handler : INotificationHandler<PowerPillEatenEvent>
        {
            readonly IGame _game;
            readonly IGameStats _gameStats;
            readonly IGameSoundPlayer _gameSoundPlayer;
            readonly IMediator _mediator;
            readonly IGhostCollection _ghostCollection;

            public Handler(IGame game,
                IGameStats gameStats,
                IGameSoundPlayer gameSoundPlayer,
                IMediator mediator,
                IGhostCollection ghostCollection)
            {
                _game = game;
                _gameStats = gameStats;
                _gameSoundPlayer = gameSoundPlayer;
                _mediator = mediator;
                _ghostCollection = ghostCollection;
            }

            public async Task Handle(PowerPillEatenEvent notification, CancellationToken cancellationToken)
            {
                await _gameSoundPlayer.PowerPillEaten();

                await _gameStats.PowerPillEaten(notification.CellIndex);

                foreach (IGhost eachGhost in _ghostCollection.Ghosts)
                {
                    GhostFrightSession? frightSession = _gameStats.CurrentPlayerStats.FrightSession;
                    
                    if (frightSession == null)
                    {
                        throw new InvalidOperationException("no fright session");
                    }
                    
                    eachGhost.PowerPillEaten(frightSession);
                }

                await checkForNoMorePills();
            }

            async Task checkForNoMorePills()
            {
                if (_gameStats.CurrentPlayerStats.LevelStats.PillsRemaining == 0)
                {
                    // ReSharper disable once HeapView.BoxingAllocation
                    var act = await _mediator.Send(new GetActRequest("LevelFinishedAct"));
                    
                    await act.Reset();
                    
                    _game.SetAct(act);
                }
            }
        }
    }
}