﻿using PacMan.GameComponents.Ghosts;

namespace PacMan.GameComponents.Events;

/// <summary>
/// A Player is starting.  We're told their index (0 or 1), their stats, whether it's the demo player,
/// and their 'canvas' (their maze where each cell is cleared when eat pills).
/// </summary>
public readonly struct PlayerStartingEvent : INotification
{
    [UsedImplicitly]
    public class Handler : INotificationHandler<PlayerStartingEvent>
    {
        private readonly IHaveTheMazeCanvases _mazeCanvases;
        private readonly IGhostCollection _ghostCollection;
        private readonly IFruit _fruit;
        private readonly IMaze _maze;
        private readonly IPacMan _pacman;
        private readonly IGame _game;
        private readonly IActs _acts;
        private readonly IGameStats _gameStats;

        public Handler(
            IGhostCollection ghostCollection,
            IFruit fruit,
            IMaze maze,
            IPacMan pacman,
            IGame game,
            IActs acts,
            IGameStats gameStats,
            IHaveTheMazeCanvases mazeCanvases)
        {
            _mazeCanvases = mazeCanvases;
            _ghostCollection = ghostCollection;
            _fruit = fruit;
            _maze = maze;
            _pacman = pacman;
            _game = game;
            _acts = acts;
            _gameStats = gameStats;
        }

        public Task Handle(PlayerStartingEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var playerStats = _gameStats.CurrentPlayerStats;

                foreach (IGhost ghost in _ghostCollection.Ghosts)
                {
                    ghost.Reset();
                }

                _fruit.HandlePlayerStarted(playerStats, false);

                _maze.HandlePlayerStarted(playerStats, _mazeCanvases.GetForPlayer(playerStats.PlayerIndex));

                _pacman.HandlePlayerStarting(playerStats, false);

                var act = _acts.GetActNamed("PlayerIntroAct");
                act.Reset();

                _game.SetAct(act);

                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}