namespace PacMan.GameComponents.Ghosts;

public class GhostLogic
{
    private readonly DistanceAndDirectionComparer _distanceAndDirectionComparer = new();

    private readonly List<Direction> _availableDirections = new(4);
    private readonly List<DistanceAndDirection> _distanceAndDirections = new(4);

    private readonly IMaze _maze;
    private readonly Ghost _ghost;

    private CellIndex _lastDecisionMadeAt;

    public GhostLogic(IMaze maze, Ghost ghost)
    {
        _maze = maze;
        _ghost = ghost;
        _lastDecisionMadeAt = CellIndex.Zero;
    }

    /// Find which way to go from the ghost's next cell (in the direction of travel)
    /// to the target cell.
    public Direction GetWhichWayToGo(CellIndex targetCell)
    {
        Tile currentTile = _ghost.Tile;

        CellIndex cellPosition = currentTile.Index;

        if (cellPosition == _lastDecisionMadeAt)
        {
            return Direction.None;
        }

        Tile nextTile = currentTile.NextTileWrapped(_ghost.Direction.Next);

        // the tile might not be in bounds, e.g. the ghost could be at the bottom of the maze facing
        // up when a power pill is eaten, this will mean the next direction is down (directions are reversed)
        // which will mean the next cell is 1 below the bottom of the maze.
        if (!nextTile.Index.IsInBounds)
        {
            return _ghost.Direction.Current;
        }

        Direction decision = CalculateWhichWayToGo(nextTile, targetCell);

        _lastDecisionMadeAt = cellPosition;

        return decision;
    }

    private Direction CalculateWhichWayToGo(Tile tile, CellIndex targetCell)
    {
        CellIndex cellPosition = tile.Index;

        DirectionChoices choices = _maze.GetChoicesAtCellPosition(cellPosition);

        Direction dir = _ghost.Direction.Next;

        _availableDirections.Clear();

        if (choices.IsSet(Direction.Up) && dir != Direction.Down)
        {
            _availableDirections.Add(Direction.Up);
        }

        if (choices.IsSet(Direction.Left) && dir != Direction.Right)
        {
            _availableDirections.Add(Direction.Left);
        }

        if (choices.IsSet(Direction.Down) && dir != Direction.Up)
        {
            _availableDirections.Add(Direction.Down);
        }

        if (choices.IsSet(Direction.Right) && dir != Direction.Left)
        {
            _availableDirections.Add(Direction.Right);
        }

        if (_availableDirections.Count == 1)
        {
            return _availableDirections[0];
        }

        if (Maze.IsSpecialIntersection(cellPosition) && _ghost.State == GhostState.Normal)
        {
            var index = _availableDirections.IndexOf(Direction.Up);

            // special intersection - remove Up from choices
            if (index != -1)
            {
                _availableDirections.RemoveAt(index);
            }
        }

        if (_availableDirections.Count == 0)
        {
            // when a ghost turns to frightened, their direction is switched immediately
            // which means that their current direction is reversed.  The above
            // logic doesn't cater for this, so we just tell it to continue the way
            // it's going
            if (_ghost.State == GhostState.Frightened)
            {
                return _ghost.Direction.Current;
            }

            throw new InvalidOperationException("No choices to pick from!");
        }

        dir = PickShortest(tile, targetCell, _availableDirections);

        return dir;
    }

    private Direction PickShortest(Tile ghostTile, CellIndex targetCell, List<Direction> choices)
    {
        if (choices.Count == 0)
        {
            throw new InvalidOperationException("No choices to pick from!");
        }

        _distanceAndDirections.Clear();

        Tile targetTile = Tile.FromIndex(targetCell);

        Vector2 centerOfTarget = targetTile.CenterPos;

        foreach (var direction in choices)
        {
            var nextTileInThatDirection = ghostTile.NextTile(direction);

            var distance = Extensions.DistanceBetween(
                nextTileInThatDirection.CenterPos,
                centerOfTarget);

            _distanceAndDirections.Add(new(distance, direction));
        }

        _distanceAndDirections.Sort(_distanceAndDirectionComparer);

        return _distanceAndDirections[0].Direction;
    }

    private class DistanceAndDirectionComparer : IComparer<DistanceAndDirection>
    {
        public int Compare(DistanceAndDirection l, DistanceAndDirection r)
        {
            decimal dist = Math.Floor((decimal)(l.Distance - r.Distance));

            if (dist != 0)
            {
                return (int)dist;
            }

            int ret = WeightDir(l.Direction) - WeightDir(r.Direction);

            return ret;
        }
    }

    // From the spec: To break the tie, the ghost prefers directions in this order: up, left, down, right
    private static int WeightDir(Direction direction) =>
        direction switch
        {
            Direction.Up => 0,
            Direction.Left => 1,
            Direction.Down => 2,
            _ => 3
        };
}