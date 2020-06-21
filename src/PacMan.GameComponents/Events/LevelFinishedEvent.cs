using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PacMan.GameComponents.GameActs;
using PacMan.GameComponents.Ghosts;
using PacMan.GameComponents.Requests;

namespace PacMan.GameComponents.Events
{
    /// <summary>
    /// When all the dots have been eaten and the <see cref="LevelFinishedAct"/> has finished playing.
    /// </summary>
    public readonly struct LevelFinishedEvent : INotification
    {
        public class Handler : INotificationHandler<LevelFinishedEvent>
        {
            readonly IMediator _mediator;
            readonly IGame _game;
            readonly IGameStats _gameStats;
            readonly IHaveTheMazeCanvases _mazeCanvases;

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
                    PlayerStats playerStats = _gameStats.CurrentPlayerStats;

                    var maze = _mazeCanvases.GetForPlayer(playerStats.PlayerIndex);
                
                    await maze.Reset();

                    _gameStats.LevelFinished();

                    IntroCutScene cutScene = playerStats.LevelStats.GetLevelProps().IntroCutScene;

                    if (cutScene == IntroCutScene.None)
                    {
                        await _mediator.Publish(new PlayerStartingEvent(), cancellationToken);

                        return;
                    }

                    var act = cutScene switch
                    {
                        IntroCutScene.BigPac => await getAct("BigPacChaseAct"),
                        IntroCutScene.GhostSnagged => await getAct("GhostTearAct"),
                        IntroCutScene.TornGhostAndWorm => await getAct("TornGhostChaseAct"),
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
            async Task<IAct> getAct(string name) => await _mediator.Send(new GetActRequest(name));
        }
    }
}