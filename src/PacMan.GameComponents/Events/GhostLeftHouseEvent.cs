using MediatR;
using PacMan.GameComponents.Ghosts;

namespace PacMan.GameComponents.Events
{
    public readonly struct GhostLeftHouseEvent : INotification
    {
        public IGhost Ghost { get; }

        public GhostLeftHouseEvent(IGhost ghost)
        {
            Ghost = ghost;
        }
    }
}