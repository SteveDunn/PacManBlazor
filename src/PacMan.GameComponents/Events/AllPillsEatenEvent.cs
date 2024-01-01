namespace PacMan.GameComponents.Events;

/// <summary>
/// When all the dots have been eaten, start the <see cref="LevelFinishedAct"/>.
/// </summary>
public readonly struct AllPillsEatenEvent : INotification
{
    [UsedImplicitly]
    public class Handler : INotificationHandler<AllPillsEatenEvent>
    {
        private readonly IMediator _mediator;
        private readonly IGame _game;
        private readonly IGameSoundPlayer _gameSoundPlayer;
        private readonly IPacMan _pacman;
        private readonly IGhostCollection _ghostCollection;

        public Handler(
            IMediator mediator,
            IGame game,
            IGameSoundPlayer gameSoundPlayer,
            IPacMan pacman,
            IGhostCollection ghostCollection)
        {
            _mediator = mediator;
            _game = game;
            _gameSoundPlayer = gameSoundPlayer;
            _pacman = pacman;
            _ghostCollection = ghostCollection;
        }

        public async Task Handle(AllPillsEatenEvent notification, CancellationToken cancellationToken)
        {
            await _gameSoundPlayer.Reset();

            _pacman.StartDigesting();
                
            foreach (var eachGhost in _ghostCollection.Ghosts)
            {
                eachGhost.StopMoving();
            }

            // ReSharper disable once HeapView.BoxingAllocation
            var act = await _mediator.Send(new GetActRequest("LevelFinishedAct"), cancellationToken);

            await act.Reset();

            _game.SetAct(act);
        }
    }
}