namespace PacMan.GameComponents.Ghosts;

/// Moves the ghost in a psuedo-random fashion while they are 'frightened' (i.e. blue)
public class GhostFrightenedMover : GhostMover
{
    public GhostFrightenedMover(Ghost ghost, IMaze maze) : base(ghost, GhostMovementMode.Frightened, maze, getTargetCell)
    {
    }

    private static ValueTask<CellIndex> getTargetCell()
    {
        var random = Pnrg.Value;

        CellIndex cell = (random % 4) switch
        {
            0 => new((int)MazeBounds.TopLeft.X, (int)MazeBounds.TopLeft.Y),
            1 => new(MazeBounds.Dimensions.Width, 0),
            2 => new(MazeBounds.Dimensions.Width, MazeBounds.Dimensions.Height),
            _ => new(0, MazeBounds.Dimensions.Height)
        };

        return new(cell);
    }
}