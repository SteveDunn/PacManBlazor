using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PacMan.GameComponents.Ghosts;

namespace PacMan.GameComponents.Events
{
    public readonly struct GhostInsideHouseEvent : INotification
    {
        public IGhost Ghost { get; }

        public GhostInsideHouseEvent(IGhost ghost)
        {
            Ghost = ghost;
        }

        public class Handler : INotificationHandler<GhostInsideHouseEvent>
        {
            readonly IGameStats _gameStats;

            public Handler(IGameStats gameStats)
            {
                _gameStats = gameStats;
            }

            public Task Handle(GhostInsideHouseEvent notification, CancellationToken cancellationToken)
            {
                _gameStats.HandleGhostBackInsideHouse();

                return Task.CompletedTask;
            }
        }
    }
}