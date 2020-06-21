using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using PacMan.GameComponents.GameActs;
using PacMan.GameComponents.Requests;

namespace PacMan.GameComponents.Events
{
    /// <summary>
    /// When pacman has been caught, eaten, and the player game over screen has shown
    /// and finished.
    /// </summary>
    public readonly struct GameOverEvent : INotification
    {
        [UsedImplicitly]
        public class Handler : INotificationHandler<GameOverEvent>
        {
            readonly IMediator _mediator;
            readonly IGameStorage _storage;
            readonly IGame _game;
            readonly IGameStats _gameStats;

            public Handler(IGame game, IGameStats gameStats, IMediator mediator, IGameStorage storage)
            {
                _game = game;
                _gameStats = gameStats;
                _mediator = mediator;
                _storage = storage;
            }

            public async Task Handle(GameOverEvent notification, CancellationToken cancellationToken)
            {
                await _storage.SetHighScore(_gameStats.HighScore);

                if (_gameStats.IsGameOver)
                {
                    // ReSharper disable once HeapView.BoxingAllocation
                    IAct act = await _mediator.Send(new GetActRequest("AttractAct"), cancellationToken);
                    await act.Reset();
                    
                    _game.SetAct(act);

                    return;
                }

                _gameStats.ChoseNextPlayer();

                await _mediator.Publish(new PlayerStartingEvent());
            }
        }
    }
}