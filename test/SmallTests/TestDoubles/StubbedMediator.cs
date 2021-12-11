using MediatR;

namespace SmallTests.TestDoubles;

public class StubbedMediator : IMediator
{
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = new CancellationToken()) => throw new NotImplementedException();

    public Task<object?> Send(object request, CancellationToken cancellationToken = new CancellationToken()) => throw new NotImplementedException();

    public Task Publish(object notification, CancellationToken cancellationToken = new CancellationToken()) => Task.CompletedTask;

    public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = new CancellationToken()) where TNotification : INotification => Task.CompletedTask;
}