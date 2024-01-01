namespace PacMan.GameComponents.Events;

/// <summary>
/// A Player is starting.  We're told their index (0 or 1), their stats, whether it's the demo player,
/// and their 'canvas' (their maze where each cell is cleared when eat pills).
/// </summary>
public readonly struct DemoPlayerStartingEvent : INotification
{
    public class Handler : INotificationHandler<DemoPlayerStartingEvent>
    {
        private readonly IGameSoundPlayer _gameSoundPlayer;
        private readonly IGhostCollection _ghostCollection;
        private readonly IFruit _fruit;
        private readonly IMaze _maze;
        private readonly IPacMan _pacman;
        private readonly IGame _game;
        private readonly IActs _acts;
        private readonly IGameStats _gameStats;
        private readonly IHaveTheMazeCanvases _mazeCanvases;

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