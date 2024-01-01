using PacMan.GameComponents.Ghosts;

namespace PacMan.GameComponents.Events;

/// <summary>
/// When all the dots have been eaten and the <see cref="LevelFinishedAct"/> has finished playing.
/// </summary>
public readonly struct LevelFinishedEvent : INotification
{
    [UsedImplicitly]
    public class Handler : INotificationHandler<LevelFinishedEvent>
    {
        private readonly IMediator _mediator;
        private readonly IGame _game;
        private readonly IGameStats _gameStats;
        private readonly IHaveTheMazeCanvases _mazeCanvases;

        public Handler(IMediator mediator, IGame game, IGameStats gameStats, IHaveTheMazeCanvases mazeCanvases)
        {
            _mediator = mediator;

            _game = game;

            _gameStats = gameStats;

            _mazeCanvases = mazeCanvases;
        }

        [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
        public async Task Handle(LevelFinishedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var playerStats = _gameStats.CurrentPlayerStats;

                var maze = _mazeCanvases.GetForPlayer(playerStats.PlayerIndex);

                await maze.Reset();

                _gameStats.LevelFinished();

                var cutScene = playerStats.LevelStats.GetLevelProps().CutScene;

                if (cutScene == IntroCutScene.None)
                {
                    await _mediator.Publish(new PlayerStartingEvent(), cancellationToken);

                    return;
                }

                // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
                var act = cutScene switch
                {
                    IntroCutScene.BigPac => await GetAct("BigPacChaseAct"),
                    IntroCutScene.GhostSnagged => await GetAct("GhostTearAct"),
                    IntroCutScene.TornGhostAndWorm => await GetAct("TornGhostChaseAct"),

                    // ReSharper disable once HeapView.BoxingAllocation
                    _ => throw new InvalidOperationException($"Don't know how to handle cut scene of {cutScene}")
                };

                await act.Reset();
                _game.SetAct(act);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [SuppressMessage("ReSharper", "HeapView.BoxingAllocation")]
        private async Task<IAct> GetAct(string name) => await _mediator.Send(new GetActRequest(name));
    }
}