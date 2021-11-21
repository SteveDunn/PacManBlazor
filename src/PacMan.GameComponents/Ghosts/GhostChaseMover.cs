namespace PacMan.GameComponents.Ghosts;

/// <summary>
/// Moves the ghost to the 'chase target' cell (provided by the ghost)
/// </summary>
public class GhostChaseMover : GhostMover
{
    public GhostChaseMover(
        Ghost ghost,
        IMaze maze) : base(ghost, GhostMovementMode.Chase, maze, ghost.GetChaseTarget)
    {
    }
}