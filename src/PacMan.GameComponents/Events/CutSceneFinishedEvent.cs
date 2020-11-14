using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace PacMan.GameComponents.Events
{
    /// <summary>
    /// Then the cut scene has finished playing.
    /// </summary>
    public readonly struct CutSceneFinishedEvent : INotification
    {
        public class Handler : INotificationHandler<CutSceneFinishedEvent>
        {
            readonly IMediator _mediator;

            public Handler(IMediator mediator) => _mediator = mediator;

            public async Task Handle(CutSceneFinishedEvent notification, CancellationToken cancellationToken) =>
                await _mediator.Publish(new PlayerStartingEvent(), cancellationToken);
        }
    }
}