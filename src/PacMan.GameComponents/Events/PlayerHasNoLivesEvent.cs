using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using PacMan.GameComponents.Requests;

namespace PacMan.GameComponents.Events;

/// <summary>
/// When pacman has been caught, eaten, and the player has no more lives.
/// </summary>
public readonly struct PlayerHasNoLivesEvent : INotification
{
    [UsedImplicitly]
    public class Handler : INotificationHandler<PlayerHasNoLivesEvent>
    {
        readonly IMediator _mediator;
        readonly IGame _game;

        public Handler(IGame game, IMediator mediator) => (_game, _mediator) = (game, mediator);

        public async Task Handle(PlayerHasNoLivesEvent notification, CancellationToken cancellationToken)
        {
            // ReSharper disable once HeapView.BoxingAllocation
            var act = await _mediator.Send(new GetActRequest("PlayerGameOverAct"), cancellationToken);
            await act.Reset();

            _game.SetAct(act);
        }
    }
}