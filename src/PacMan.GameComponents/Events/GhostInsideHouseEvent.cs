using PacMan.GameComponents.Ghosts;

namespace PacMan.GameComponents.Events;

public readonly struct GhostInsideHouseEvent : INotification
{
    public IGhost Ghost { get; }

    public GhostInsideHouseEvent(IGhost ghost)
    {
        Ghost = ghost;
    }

    [UsedImplicitly]
    public class Handler : INotificationHandler<GhostInsideHouseEvent>
    {
        private readonly IGameStats _gameStats;

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