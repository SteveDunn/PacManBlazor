using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PacMan.GameComponents.Ghosts;

namespace PacMan.GameComponents.Requests;

public readonly struct GetGameStateRequest : IRequest<GameState>
{
    public class Handler : IRequestHandler<GetGameStateRequest, GameState>
    {
        readonly IGhostCollection _ghostCollection;

        public Handler(IGhostCollection ghostCollection)
        {
            _ghostCollection = ghostCollection;
        }

        public Task<GameState> Handle(GetGameStateRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new GameState
            {
                IsClydeInHouse = _ghostCollection.GetGhost(GhostNickname.Clyde).IsInHouse
            });
        }
    }
}