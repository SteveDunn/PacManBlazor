using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PacMan.GameComponents.Audio;

namespace PacMan.GameComponents.Events;

public readonly struct FruitEatenEvent : INotification
{
    public IFruit Fruit { get; }

    public FruitEatenEvent(IFruit fruit)
    {
        Fruit = fruit;
    }

    public class Handler : INotificationHandler<FruitEatenEvent>
    {
        readonly IGame _game;
        readonly IGameStats _gameStats;
        readonly IGameSoundPlayer _gameSoundPlayer;

        public Handler(IGame game, IGameStats gameStats, IGameSoundPlayer gameSoundPlayer)
        {
            _game = game;
            _gameStats = gameStats;
            _gameSoundPlayer = gameSoundPlayer;
        }

        public async Task Handle(FruitEatenEvent notification, CancellationToken cancellationToken)
        {
            await _gameSoundPlayer.FruitEaten();

            await _gameStats.FruitEaten();

            int points = _gameStats.CurrentPlayerStats.LevelStats.GetLevelProps().FruitPoints;

            await _game.FruitEaten(points);
        }
    }
}

public readonly struct ExtraLifeEvent : INotification
{
    public class Handler : INotificationHandler<ExtraLifeEvent>
    {
        readonly IGameSoundPlayer _gameSoundPlayer;

        public Handler(IGameSoundPlayer gameSoundPlayer)
        {
            _gameSoundPlayer = gameSoundPlayer;
        }

        public async Task Handle(ExtraLifeEvent notification, CancellationToken cancellationToken) =>
            await _gameSoundPlayer.GotExtraLife();
    }
}