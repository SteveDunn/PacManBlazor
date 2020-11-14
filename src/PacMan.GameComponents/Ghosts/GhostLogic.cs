using System;
using System.Collections.Generic;
using System.Numerics;

namespace PacMan.GameComponents.Ghosts
{
    public class GhostLogic
    {
        readonly DistanceAndDirectionComparer _distanceAndDirectionComparer = new();

        readonly List<Directions> _availableDirections = new(4);
        readonly List<DistanceAndDirection> _distanceAndDirections = new(4);

        readonly IMaze _maze;
        readonly Ghost _ghost;

        CellIndex _lastDecisionMadeAt;

        public GhostLogic(IMaze maze, Ghost ghost)
        {
            _maze = maze;
            _ghost = ghost;
            _lastDecisionMadeAt = CellIndex.Zero;
        }

        /// Find which way to go from the ghost's next cell (in the direction of travel)
        /// to the target cell.
        public Directions GetWhichWayToGo(CellIndex targetCell)
        {
            Tile currentTile = _ghost.Tile;

            CellIndex cellPosition = currentTile.Index;

            if (cellPosition == _lastDecisionMadeAt)
            {
                return Directions.None;
            }

            Tile nextTile = currentTile.NextTileWrapped(_ghost.Direction.Next);

            // the tile might not be in bounds, e.g. the ghost could be at the bottom of the maze facing
            // up when a power pill is eaten, this will mean the next direction is down (directions are reversed)
            // which will mean the next cell is 1 below the bottom of the maze.
            if (!nextTile.Index.IsInBounds)
            {
                return _ghost.Direction.Current;
            }

            Directions decision = calculateWhichWayToGo(nextTile, targetCell);

            _lastDecisionMadeAt = cellPosition;

            return decision;
        }

        Directions calculateWhichWayToGo(Tile tile, CellIndex targetCell)
        {
            CellIndex cellPosition = tile.Index;

            DirectionChoices choices = _maze.GetChoicesAtCellPosition(cellPosition);

            Directions dir = _ghost.Direction.Next;

            _availableDirections.Clear();

            if (choices.IsSet(Directions.Up) && dir != Directions.Down)
            {
                _availableDirections.Add(Directions.Up);
            }

            if (choices.IsSet(Directions.Left) && dir != Directions.Right)
            {
                _availableDirections.Add(Directions.Left);
            }

            if (choices.IsSet(Directions.Down) && dir != Directions.Up)
            {
                _availableDirections.Add(Directions.Down);
            }

            if (choices.IsSet(Directions.Right) && dir != Directions.Left)
            {
                _availableDirections.Add(Directions.Right);
            }

            if (_availableDirections.Count == 1)
            {
                return _availableDirections[0];
            }

            if (Maze.IsSpecialIntersection(cellPosition) && _ghost.State == GhostState.Normal)
            {
                var index = _availableDirections.IndexOf(Directions.Up);

                // special intersection - remove Up from choices
                if (index != -1)
                {
                    _availableDirections.RemoveAt(index);
                }
            }

            if (_availableDirections.Count == 0)
            {
                // when a ghost turns to frightened, their direction is switched immediately
                // which means that they're current direction is reversed.  The above
                // logic doesn't cater for this, so we just tell it to continue the way
                // it's going
                if (_ghost.State == GhostState.Frightened)
                {
                    return _ghost.Direction.Current;
                }

                throw new InvalidOperationException("No choices to pick from!");
            }

            dir = pickShortest(tile, targetCell, _availableDirections);

            return dir;
        }

        Directions pickShortest(Tile ghostTile, CellIndex targetCell, List<Directions> choices)
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

        class DistanceAndDirectionComparer : IComparer<DistanceAndDirection>
        {
            public int Compare(DistanceAndDirection l, DistanceAndDirection r)
            {
                decimal dist = Math.Floor((decimal)(l.Distance - r.Distance));

                if (dist != 0)
                {
                    return (int)dist;
                }

                int ret = weightDir(l.Direction) - weightDir(r.Direction);

                return ret;
            }
        }

        // From the spec: To break the tie, the ghost prefers directions in this order: up, left, down, right
        static int weightDir(Directions direction) =>
            direction switch
            {
                Directions.Up => 0,
                Directions.Left => 1,
                Directions.Down => 2,
                _ => 3
            };
    }
}