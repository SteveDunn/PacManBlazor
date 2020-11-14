using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PacMan.GameComponents.Audio;

namespace PacMan.GameComponents.Events
{
    public readonly struct PillEatenEvent : INotification
    {
        public PillEatenEvent(CellIndex cellIndex)
        {
            CellIndex = cellIndex;
        }

        public CellIndex CellIndex { get; }

        public class Handler : INotificationHandler<PillEatenEvent>
        {
            readonly IGameStats _gameStats;
            readonly IGameSoundPlayer _gameSoundPlayer;
            readonly IPacMan _pacman;
            readonly IMediator _mediator;

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
                await checkForNoMorePills();
            }

            async Task checkForNoMorePills()
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
}