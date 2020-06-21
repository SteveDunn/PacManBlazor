using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using PacMan.GameComponents.GameActs;
using PacMan.GameComponents.Requests;

namespace PacMan.GameComponents.Events
{
    /// <summary>
    /// When PacMan is dead (he's been caught by a ghost and eaten)...
    /// 1. If he died in a demo level, then show the attract screen
    /// 2. If all players are dead, or the current player has no lives, then show player game over screen
    /// 3. Otherwise, show the player intro act
    /// </summary>
    public readonly struct PacManDeadEvent : INotification
    {
        [UsedImplicitly]
        public class Handler : INotificationHandler<PacManDeadEvent>
        {
            readonly IMediator _mediator;
            readonly IGame _game;
            readonly IGameStats _gameStats;

            public Handler(IGame game, IGameStats gameStats, IMediator mediator)
            {
                _game = game;
                _gameStats = gameStats;
                _mediator = mediator;
            }

            [SuppressMessage("ReSharper", "HeapView.BoxingAllocation")]
            public async Task Handle(PacManDeadEvent notification, CancellationToken cancellationToken)
            {
                if (_gameStats.IsDemo)
                {
                    IAct act = await _mediator.Send(new GetActRequest("AttractAct"), cancellationToken);
                    
                    await act.Reset();
                    
                    _game.SetAct(act);

                    return;
                }

                PlayerStats currentPlayerStats = _gameStats.CurrentPlayerStats;

                if (currentPlayerStats.LivesRemaining == 0 || _gameStats.IsGameOver)
                {
                    await _mediator.Publish(new PlayerHasNoLivesEvent(), cancellationToken);
                    
                    return;
                }

                _gameStats.ChoseNextPlayer();

                await _mediator.Publish(new PlayerStartingEvent(), cancellationToken);
            }
        }
    }
}