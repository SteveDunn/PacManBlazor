using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PacMan.GameComponents.GameActs;

namespace PacMan.GameComponents.Requests
{
    public readonly struct GetActRequest : IRequest<IAct>
    {
        public GetActRequest(string actName)
        {
            ActName = actName;
        }

        public string ActName { get; }

        public class Handler : IRequestHandler<GetActRequest, IAct>
        {
            readonly IActs _acts;

            public Handler(IActs acts)
            {
                _acts = acts;
            }

            public Task<IAct> Handle(GetActRequest request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_acts.GetActNamed(request.ActName));
            }
        }
    }
}