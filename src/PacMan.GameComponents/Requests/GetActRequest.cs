namespace PacMan.GameComponents.Requests;

public readonly struct GetActRequest : IRequest<IAct>
{
    public GetActRequest(string actName)
    {
        ActName = actName;
    }

    public string ActName { get; }

    public class Handler : IRequestHandler<GetActRequest, IAct>
    {
        private readonly IActs _acts;

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