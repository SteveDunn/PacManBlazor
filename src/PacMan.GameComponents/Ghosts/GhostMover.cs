namespace PacMan.GameComponents.Ghosts;

/// <summary>
///  Represents the different ghost movers.  Ghost movements are either:
/// * Chase (chase after Pacman),
/// * Scatter (they scatter back to their 'home corners')
/// * Frightened (run away from Pacman)
/// * 'Eyes back to house' (they've been eaten by Pacman and are making their way back to the 'house')
/// * 'Inside house' (they're inside the house waiting to come out).
/// </summary>
public abstract class GhostMover
{
    public CellIndex TargetCell { get; private set; }

    public GhostMovementMode MovementMode { get; }

    protected Ghost Ghost { get; }

    readonly Func<ValueTask<CellIndex>> _getTargetCellPoint;
    readonly GhostLogic _intersectionLogic;

    protected GhostMover(
        Ghost ghost,
        GhostMovementMode movementMode,
        IMaze maze,
        Func<ValueTask<CellIndex>> getTargetCellPoint)
    {
        _getTargetCellPoint = getTargetCellPoint;
        Ghost = ghost;
        MovementMode = movementMode;
        _intersectionLogic = new(maze, ghost);
    }

    public async virtual ValueTask<MovementResult> Update(CanvasTimingInformation context)
    {
        var tile = Ghost.Tile;

        // if a ghost is near the center of a cell, then get the 'next cell' and
        // store where to go from there

        if (tile.IsInCenter)
        {
            CellIndex targetCell = await _getTargetCellPoint();
            TargetCell = targetCell;

            Direction direction = _intersectionLogic.GetWhichWayToGo(targetCell);

            if (direction != Direction.None)
            {
                SetDirection(direction);
            }
        }

        Ghost.MoveForwards();

        return MovementResult.NotFinished;
    }

    void SetDirection(Direction direction)
    {
        Ghost.Direction.Update(direction);
    }
}