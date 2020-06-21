namespace PacMan.GameComponents.Ghosts
{
    /// <summary>
    /// Moves the ghost to their 'scatter cell' (provided by the ghost)
    /// </summary>
    public class GhostScatterMover : GhostMover
    {
        public GhostScatterMover(Ghost ghost, IMaze maze) : base(
            ghost,
            GhostMovementMode.Scatter,
            maze,
            ghost.GetScatterTarget)
        {
        }
    }
}