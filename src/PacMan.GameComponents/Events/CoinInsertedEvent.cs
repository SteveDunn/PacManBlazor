namespace PacMan.GameComponents.Events;

public readonly struct CoinInsertedEvent : INotification
{
    [UsedImplicitly]
    public class Handler : INotificationHandler<CoinInsertedEvent>
    {
        readonly IGameSoundPlayer _gameSoundPlayer;
        readonly ICoinBox _coinBox;
        readonly IGame _game;
        readonly IActs _acts;

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