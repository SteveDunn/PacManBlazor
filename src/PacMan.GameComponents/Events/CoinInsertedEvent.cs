namespace PacMan.GameComponents.Events;

public readonly struct CoinInsertedEvent : INotification
{
    [UsedImplicitly]
    public class Handler : INotificationHandler<CoinInsertedEvent>
    {
        private readonly IGameSoundPlayer _gameSoundPlayer;
        private readonly ICoinBox _coinBox;
        private readonly IGame _game;
        private readonly IActs _acts;

        public Handler(IGameSoundPlayer gameSoundPlayer, ICoinBox coinBox, IGame game, IActs acts)
        {
            _gameSoundPlayer = gameSoundPlayer;
            _coinBox = coinBox;
            _game = game;
            _acts = acts;
        }

        public async Task Handle(CoinInsertedEvent notification, CancellationToken cancellationToken)
        {
            _coinBox.CoinInserted();

            await _gameSoundPlayer.CoinInserted();

            IAct currentAct = _acts.GetActNamed("StartButtonAct");

            _game.SetAct(currentAct);
        }
    }
}