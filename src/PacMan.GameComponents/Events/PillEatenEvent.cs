namespace PacMan.GameComponents.Events;

public readonly struct PillEatenEvent : INotification
{
    public PillEatenEvent(CellIndex cellIndex)
    {
        CellIndex = cellIndex;
    }

    public CellIndex CellIndex { get; }

    [UsedImplicitly]
    public class Handler : INotificationHandler<PillEatenEvent>
    {
        private readonly IGameStats _gameStats;
        private readonly IGameSoundPlayer _gameSoundPlayer;
        private readonly IPacMan _pacman;
        private readonly IMediator _mediator;

        public Handler(IGameStats gameStats, IGameSoundPlayer gameSoundPlayer, IPacMan pacman, IMediator mediator)
        {
            _gameStats = gameStats;
            _gameSoundPlayer = gameSoundPlayer;
            _pacman = pacman;
            _mediator = mediator;
        }

        public async Task Handle(PillEatenEvent notification, CancellationToken cancellationToken)
        {
            if (_gameStats.CurrentPlayerStats.LevelStats.PillsEaten % 2 == 0)
            {
                await _gameSoundPlayer.Munch1();
            }
            else
            {
                await _gameSoundPlayer.Munch2();
            }

            await _gameStats.PillEaten(notification.CellIndex);

            _pacman.PillEaten();
            await CheckForNoMorePills();
        }

        private async Task CheckForNoMorePills()
        {
            if (_gameStats.CurrentPlayerStats.LevelStats.PillsRemaining == 0)
            {
                // _gameStats.levelFinished();
                // don't call levelFinished - the act does that when it's finished

                await _mediator.Publish(new AllPillsEatenEvent());
            }
        }
    }
}