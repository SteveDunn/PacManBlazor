namespace PacMan.GameComponents.Events;

public readonly struct DemoStartedEvent : INotification
{
    [UsedImplicitly]
    public class Handler : INotificationHandler<DemoStartedEvent>
    {
        readonly IGameSoundPlayer _gameSoundPlayer;
        readonly IGameStats _gameStats;
        readonly IMediator _mediator;
        readonly IHaveTheMazeCanvases _mazeCanvases;

        public Handler(
            IGameSoundPlayer gameSoundPlayer,
            IGameStats gameStats,
            IMediator mediator,
            IHaveTheMazeCanvases mazeCanvases)
        {
            _gameSoundPlayer = gameSoundPlayer;
            _gameStats = gameStats;
            _mediator = mediator;
            _mazeCanvases = mazeCanvases;
        }

        public async Task Handle(DemoStartedEvent notification, CancellationToken cancellationToken)
        {
            _gameStats.ResetForDemo();

            await _gameSoundPlayer.Disable();

            var canvasWrapper = _mazeCanvases.GetForPlayer(0);
            await canvasWrapper.Reset();

            var playerStartedEvent = new DemoPlayerStartingEvent();

            await _mediator.Publish(playerStartedEvent, cancellationToken);
        }
    }
}