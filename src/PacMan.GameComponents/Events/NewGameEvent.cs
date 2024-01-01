namespace PacMan.GameComponents.Events;

public readonly struct NewGameEvent : INotification
{
    public int AmountOfPlayers { get; }

    public NewGameEvent(in int amountOfPlayers)
    {
        AmountOfPlayers = amountOfPlayers;
    }

    [UsedImplicitly]
    public class Handler : INotificationHandler<NewGameEvent>
    {
        private readonly IMediator _mediator;
        private readonly ICoinBox _coinBox;
        private readonly IHaveTheMazeCanvases _mazeCanvases;
        private readonly IGameStats _gameStats;

        public Handler(
            IGameStats gameStats,
            IMediator mediator,
            ICoinBox coinBox,
            IHaveTheMazeCanvases mazeCanvases)
        {
            _gameStats = gameStats;
            _mediator = mediator;
            _coinBox = coinBox;
            _mazeCanvases = mazeCanvases;
        }

        public async Task Handle(NewGameEvent notification, CancellationToken cancellationToken)
        {
            await _mazeCanvases.GetForPlayer(0).Reset();
            await _mazeCanvases.GetForPlayer(1).Reset();

            _coinBox.UseCredits(notification.AmountOfPlayers);

            await _gameStats.Reset(notification.AmountOfPlayers);
            _gameStats.ChoseNextPlayer();

            var playerStartingEvent = new PlayerStartingEvent();

            await _mediator.Publish(playerStartingEvent, cancellationToken);
        }
    }
}