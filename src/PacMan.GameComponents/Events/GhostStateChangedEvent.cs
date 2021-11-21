using MediatR;
using PacMan.GameComponents.Ghosts;

namespace PacMan.GameComponents.Events;

public readonly struct GhostStateChangedEvent : INotification
{
    public IGhost Ghost { get; }

    public GhostState State { get; }

    public GhostStateChangedEvent(IGhost ghost, GhostState state)
    {
        Ghost = ghost;
        State = state;
    }
}