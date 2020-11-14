using MediatR;
using PacMan.GameComponents.Ghosts;

namespace PacMan.GameComponents.Events
{
    public readonly struct GhostMovementModeChangedEvent : INotification
    {
        public IGhost Ghost { get; }

        public GhostMovementMode Mode { get; }

        public GhostMovementModeChangedEvent(IGhost ghost, GhostMovementMode mode)
        {
            Ghost = ghost;
            Mode = mode;
        }
    }
}