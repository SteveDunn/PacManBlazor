using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using PacMan.GameComponents.Audio;
using PacMan.GameComponents.GameActs;
using PacMan.GameComponents.Requests;

namespace PacMan.GameComponents.Events
{
    /// <summary>
    /// When all the dots have been eaten, start the <see cref="LevelFinishedAct"/>
    /// </summary>
    public readonly struct AllPillsEatenEvent : INotification
    {
        [UsedImplicitly]
        public class Handler : INotificationHandler<AllPillsEatenEvent>
        {
            readonly IMediator _mediator;
            readonly IGame _game;
            readonly IGameSoundPlayer _gameSoundPlayer;
            readonly IPacMan _pacman;
            readonly IGhostCollection _ghostCollection;

            public Handler(IMediator mediator, IGame game, IGameSoundPlayer gameSoundPlayer, IPacMan pacman, IGhostCollection ghostCollection)
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

                _ghostCollection.Ghosts.ForEach(g => g.StopMoving());
                
                // ReSharper disable once HeapView.BoxingAllocation
                var act = await _mediator.Send(new GetActRequest("LevelFinishedAct"), cancellationToken);
                    
                await act.Reset();
                    
                _game.SetAct(act);
            }
        }
    }
}