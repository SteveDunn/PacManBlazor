using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MediatR;
using PacMan.GameComponents.Audio;
using PacMan.GameComponents.Canvas;
using PacMan.GameComponents.Requests;

namespace PacMan.GameComponents.GameActs
{
    /// <summary>
    /// Introduces the current player, shows player X and ready for 3 seconds, then ghosts and ready for 3 seconds.
    /// Transitions to either the 'demo act' (if in demo mode), otherwise the 'game act'
    /// </summary>
    public class PlayerIntroAct : IAct
    {
        readonly IMediator _mediator;
        readonly IGhostCollection _ghostCollection;
        readonly IGameStats _gameStats;
        readonly IGameSoundPlayer _gameSoundPlayer;
        readonly IPacMan _pacman;
        readonly IMaze _maze;
        readonly IGame _game;
        int _progress;
        bool _finished;

        LoopingTimer _currentTimer = LoopingTimer.DoNothing;

        public PlayerIntroAct(
            IMediator mediator,
            IGhostCollection ghostCollection,
            IGameStats gameStats,
            IGameSoundPlayer gameSoundPlayer,
            IPacMan pacman,
            IMaze maze,
            IGame game)
        {
            _mediator = mediator;
            _ghostCollection = ghostCollection;
            _gameStats = gameStats;
            _gameSoundPlayer = gameSoundPlayer;
            _pacman = pacman;
            _maze = maze;
            _game = game;
        }

        public string Name => "PlayerIntroAct";

        [SuppressMessage("ReSharper", "HeapView.BoxingAllocation")]
        [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
        public ValueTask Reset()
        {
            _finished = false;

            _progress = 0;

            var timeToShowPlayerNumberAndHideGhosts = 2.Seconds();

            if (!_gameStats.HasPlayedIntroTune)
            {
                _gameStats.HasPlayedIntroTune = true;
                _ = _gameSoundPlayer.PlayerStart();
            }

            _currentTimer = new(timeToShowPlayerNumberAndHideGhosts, () =>
            {
                _progress += 1;

                _gameStats.CurrentPlayerStats.DecreaseLives();

                _currentTimer = new(2.Seconds(), async () =>
                {
                    _finished = true;

                    var act = await _mediator.Send(new GetActRequest("GameAct"));

                    _game.SetAct(act);
                });
            });

            return default;
        }

        public ValueTask<ActUpdateResult> Update(CanvasTimingInformation timing)
        {
            if (_finished)
            {
                return new(ActUpdateResult.Running);
            }

            _currentTimer.Run(timing);

            return new(ActUpdateResult.Running);
        }

        public async ValueTask Draw(CanvasWrapper session)
        {
            await _maze.Draw(session);

            await drawPlayerAndReadyText(session);

            if (_progress == 1)
            {
                await _pacman.Draw(session);

                await _ghostCollection.DrawAll(session);
            }
        }

        async ValueTask drawPlayerAndReadyText(CanvasWrapper canvas)
        {
            if (_progress == 0)
            {
                var text = _gameStats.CurrentPlayerStats.PlayerIndex == 0 ? "PLAYER ONE" : "PLAYER TWO";

                await canvas.DrawMyText(text, TextPoints.PlayerTextPoint, Colors.Cyan);
            }

            await canvas.DrawMyText("READY!", TextPoints.ReadyPoint, Colors.Yellow);
        }
    }
}