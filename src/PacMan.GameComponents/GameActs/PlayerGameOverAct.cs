using System.Threading.Tasks;
using MediatR;
using PacMan.GameComponents.Canvas;
using PacMan.GameComponents.Events;
using PacMan.GameComponents.Requests;

namespace PacMan.GameComponents.GameActs
{
    /// Draws game over and player X
    public class PlayerGameOverAct : IAct
    {
        readonly IMediator _mediator;
        readonly IGameStats _gameStats;
        
        IAct? _gameOverAct;

        public PlayerGameOverAct(IMediator mediator, IGameStats gameStats)
        {
            _mediator = mediator;
            _gameStats = gameStats;
        }

        async Task<IAct> resolveGameOverAct()
        {
            if (_gameOverAct != null)
            {
                return _gameOverAct;
            }

            // ReSharper disable once HeapView.BoxingAllocation
            _gameOverAct = await _mediator.Send(new GetActRequest("GameOverAct"));

            return _gameOverAct;
        }

        public string Name => "PlayerGameOverAct";
        
        public async ValueTask Reset()
        {
            var a = await resolveGameOverAct();
            
            await a.Reset();
        }

        public async ValueTask<ActUpdateResult> Update(CanvasTimingInformation timing)
        {
            var gameOverAct = await resolveGameOverAct();

            var result = await gameOverAct.Update(timing);

            if (result == ActUpdateResult.Finished)
            {
                await _mediator.Publish(new GameOverEvent());
            }
            
            return result;
        }

        public async ValueTask Draw(CanvasWrapper session)
        {
            PlayerStats currentPlayerStats = _gameStats.CurrentPlayerStats;

            var gameOverAct = await resolveGameOverAct();

            await gameOverAct.Draw(session);

            await session.DrawMyText(
                currentPlayerStats.PlayerIndex == 0 ? "PLAYER ONE" : "PLAYER TWO", TextPoints.PlayerTextPoint,
                Colors.Cyan);
        }
    }
}