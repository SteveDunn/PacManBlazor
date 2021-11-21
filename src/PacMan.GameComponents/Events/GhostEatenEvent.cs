using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PacMan.GameComponents.Audio;
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
        readonly IGame _game;
        readonly IGameStats _gameStats;
        readonly IGameSoundPlayer _gameSoundPlayer;

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