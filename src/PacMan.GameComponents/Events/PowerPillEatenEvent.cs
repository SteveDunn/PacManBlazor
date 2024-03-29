﻿using PacMan.GameComponents.Ghosts;

namespace PacMan.GameComponents.Events;

public readonly struct PowerPillEatenEvent : INotification
{
    public PowerPillEatenEvent(CellIndex cellIndex) => CellIndex = cellIndex;

    public CellIndex CellIndex { get; }

    [UsedImplicitly]
    public class Handler : INotificationHandler<PowerPillEatenEvent>
    {
        private readonly IGame _game;
        private readonly IGameStats _gameStats;
        private readonly IGameSoundPlayer _gameSoundPlayer;
        private readonly IMediator _mediator;
        private readonly IGhostCollection _ghostCollection;

        public Handler(
            IGame game,
            IGameStats gameStats,
            IGameSoundPlayer gameSoundPlayer,
            IMediator mediator,
            IGhostCollection ghostCollection)
        {
            _game = game;
            _gameStats = gameStats;
            _gameSoundPlayer = gameSoundPlayer;
            _mediator = mediator;
            _ghostCollection = ghostCollection;
        }

        public async Task Handle(PowerPillEatenEvent notification, CancellationToken cancellationToken)
        {
            await _gameSoundPlayer.PowerPillEaten();

            await _gameStats.PowerPillEaten(notification.CellIndex);
                
            GhostFrightSession? frightSession = _gameStats.CurrentPlayerStats.FrightSession;

            if (frightSession == null)
            {
                throw new InvalidOperationException("no fright session");
            }

            foreach (IGhost eachGhost in _ghostCollection.Ghosts)
            {
                eachGhost.PowerPillEaten(frightSession);
            }

            await CheckForNoMorePills();
        }

        private async Task CheckForNoMorePills()
        {
            if (_gameStats.CurrentPlayerStats.LevelStats.PillsRemaining == 0)
            {
                // ReSharper disable once HeapView.BoxingAllocation
                var act = await _mediator.Send(new GetActRequest("LevelFinishedAct"));

                await act.Reset();

                _game.SetAct(act);
            }
        }
    }
}