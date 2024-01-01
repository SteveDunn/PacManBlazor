using PacMan.GameComponents.Ghosts;

namespace PacMan.GameComponents.Events;

public readonly struct GhostEatenEvent : INotification
{
    public IGhost Ghost { get; }

    public GhostEatenEvent(IGhost ghost)
    {
        Ghost = ghost;
    }

    public class Handler : INotificationHandler<GhostEatenEvent>
    {
        private readonly IGame _game;
        private readonly IGameStats _gameStats;
        private readonly IGameSoundPlayer _gameSoundPlayer;

        public Handler(IGame game, IGameStats gameStats, IGameSoundPlayer gameSoundPlayer)
        {
            _game = game;
            _gameStats = gameStats;
            _gameSoundPlayer = gameSoundPlayer;
        }

        public async Task Handle(GhostEatenEvent notification, CancellationToken cancellationToken)
        {
            await _gameSoundPlayer.GhostEaten();

            var points = await _gameStats.GhostEaten();

            await _game.GhostEaten(notification.Ghost, points);
        }
    }
}