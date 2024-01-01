namespace PacMan.GameComponents.Events;

/// <summary>
/// When PacMan is touched by a Ghost.  Shows the 'dying sequence', which then fires another
/// event (<see cref="PacManDeadEvent"/>)
/// </summary>
public readonly struct PacManEatenEvent : INotification
{
    [UsedImplicitly]
    public class Handler : INotificationHandler<PacManEatenEvent>
    {
        private readonly IMediator _mediator;
        private readonly IGame _game;
        private readonly IGameStats _gameStats;

        public Handler(IGame game, IGameStats gameStats, IMediator mediator)
        {
            _game = game;
            _gameStats = gameStats;
            _mediator = mediator;
        }

        public async Task Handle(PacManEatenEvent notification, CancellationToken cancellationToken)
        {
            _gameStats.PacManEaten();

            if (!Cheats.PacManNeverDies)
            {
                // ReSharper disable once HeapView.BoxingAllocation
                var dyingAct = await _mediator.Send(new GetActRequest("PacManDyingAct"), cancellationToken);

                await dyingAct.Reset();

                _game.SetAct(dyingAct);
            }
        }
    }
}