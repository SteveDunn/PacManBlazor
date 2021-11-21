using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PacMan.GameComponents.Audio;
using PacMan.GameComponents.GameActs;

namespace PacMan.GameComponents.Events;

/// <summary>
/// A Player is starting.  We're told their index (0 or 1), their stats, whether it's the demo player,
/// and their 'canvas' (their maze where each cell is cleared when eat pills).
/// </summary>
public readonly struct DemoPlayerStartingEvent : INotification
{
    public class Handler : INotificationHandler<DemoPlayerStartingEvent>
    {
        readonly IGameSoundPlayer _gameSoundPlayer;
        readonly IGhostCollection _ghostCollection;
        readonly IFruit _fruit;
        readonly IMaze _maze;
        readonly IPacMan _pacman;
        readonly IGame _game;
        readonly IActs _acts;
        readonly IGameStats _gameStats;
        readonly IHaveTheMazeCanvases _mazeCanvases;

        public Handler(
            IGameSoundPlayer gameSoundPlayer,
            IGhostCollection ghostCollection,
            IFruit fruit,
            IMaze maze,
            IPacMan pacman,
            IGame game,
            IActs acts,
            IGameStats gameStats,
            IHaveTheMazeCanvases mazeCanvases)
        {
            _gameSoundPlayer = gameSoundPlayer;
            _ghostCollection = ghostCollection;
            _fruit = fruit;
            _maze = maze;
            _pacman = pacman;
            _game = game;
            _acts = acts;
            _gameStats = gameStats;
            _mazeCanvases = mazeCanvases;
        }

        public async Task Handle(DemoPlayerStartingEvent notification, CancellationToken cancellationToken)
        {
            var playerStats = _gameStats.CurrentPlayerStats;

            await _gameSoundPlayer.Disable();

            _ghostCollection.Ghosts.ForEach(g => g.Reset());

            _fruit.HandlePlayerStarted(playerStats, true);

            await _maze.HandlePlayerStarted(
                playerStats,
                _mazeCanvases.GetForPlayer(_gameStats.CurrentPlayerStats.PlayerIndex));

            await _pacman.HandlePlayerStarting(playerStats, true);

            var act = _acts.GetActNamed("DemoPlayerIntroAct");
            await act.Reset();

            _game.SetAct(act);
        }
    }
}