using System.Numerics;
using PacMan.GameComponents.Canvas;

namespace PacMan.GameComponents.Ghosts;

/// Moves the ghosts while they are inside of the house
public class GhostInsideHouseMover : GhostMover
{
    readonly GhostHouseDoor _door;
    readonly Vector2[] _routeOut;
    readonly Vector2 _topPos;
    readonly Vector2 _bottomPos;

    Vector2 _cellToMoveFrom;
    Vector2 _cellToMoveTo;

    bool _readyToExit;
    int _indexInRouteOut;
    bool _finished;

    public GhostInsideHouseMover(
        Ghost ghost,
        IMaze maze,
        GhostHouseDoor ghostHouseDoor) : base(ghost, GhostMovementMode.InHouse, maze, () => new(CellIndex.Zero))
    {
        _door = ghostHouseDoor;

        Vector2 center = Maze.PixelCenterOfHouse;

        float x = center.X + (ghost.OffsetInHouse * 16);

        _topPos = new(x, (float)((13.5 * 8) + 4));
        _bottomPos = new(x, (float)((15.5 * 8) - 4));

        var centerOfUpDown = new Vector2(_topPos.X, Maze.PixelCenterOfHouse.Y);

        ghost.Position = _topPos + new Vector2(0, (_bottomPos.Y - _topPos.Y) / 2);

        _cellToMoveFrom = ghost.Position;

        if (ghost.Direction.Current == Direction.Down)
        {
            _cellToMoveTo = _bottomPos;
        }
        else if (ghost.Direction.Current == Direction.Up)
        {
            _cellToMoveTo = _topPos;
        }
        else
        {
            throw new InvalidOperationException("Ghost must be pointing up or down at start.");
        }

        _routeOut = new[] { centerOfUpDown, Maze.PixelCenterOfHouse, Maze.PixelHouseEntrancePoint };
    }

    void whenAtTargetCell()
    {
        _cellToMoveFrom = _cellToMoveTo;

        if (!_readyToExit)
        {
            if (_cellToMoveTo == _topPos)
            {
                _cellToMoveTo = _bottomPos;
            }
            else
            {
                _cellToMoveTo = _topPos;
            }

            return;
        }

        if (_indexInRouteOut == _routeOut.Length)
        {
            _finished = true;
        }
        else
        {
            _cellToMoveTo = _routeOut[_indexInRouteOut++];
        }
    }

    public override ValueTask<MovementResult> Update(CanvasTimingInformation context)
    {
        if (!_readyToExit && _door.CanGhostLeave(Ghost))
        {
            _readyToExit = true;
            return new(MovementResult.NotFinished);
        }

        if (_finished)
        {
            Ghost.Direction = new(Direction.Left, Direction.Left);
            Ghost.SetMovementMode(GhostMovementMode.Undecided);
            return new(MovementResult.Finished);
        }

        Vector2 diff = _cellToMoveTo - _cellToMoveFrom;

        if (diff != Vector2.Zero)
        {
            diff = diff.Normalize();

            Ghost.Position = Ghost.Position + (diff / Vector2s.Two);

            var dir = DirectionToIndexLookup.GetDirectionFromVector(diff);

            Ghost.Direction = new(dir, dir);
        }

        if (Ghost.Position.Floor() == _cellToMoveTo.Floor())
        {
            Ghost.Position = _cellToMoveTo;
            whenAtTargetCell();
        }

        return new(MovementResult.NotFinished);
    }
}