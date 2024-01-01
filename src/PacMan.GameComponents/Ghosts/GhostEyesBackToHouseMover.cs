namespace PacMan.GameComponents.Ghosts;

/// Moves the ghost back to the house.
public class GhostEyesBackToHouseMover : GhostMover
{
    private readonly IMediator _mediator;
    private readonly Vector2 _ghostPosInHouse;
    private Func<CanvasTimingInformation, ValueTask<MovementResult>> _currentAction;

    public GhostEyesBackToHouseMover(Ghost ghost, IMaze maze, IMediator mediator)
        : base(ghost, GhostMovementMode.GoingToHouse, maze, () => new(Maze.TileHouseEntrance.ToCellIndex()))
    {
        _mediator = mediator;
        _ghostPosInHouse = Maze.PixelCenterOfHouse + new Vector2(ghost.OffsetInHouse * 16, 0);

        _currentAction = NavigateEyesBackToJustOutsideHouse;
    }

    private async ValueTask<MovementResult> NavigateEyesBackToJustOutsideHouse(CanvasTimingInformation context)
    {
        await base.Update(context);

        if (IsNearHouseEntrance())
        {
            await _mediator.Publish(new GhostInsideHouseEvent());
            Ghost.Position = Maze.PixelHouseEntrancePoint;
            _currentAction = NavigateToCenterOfHouse;
        }

        return MovementResult.NotFinished;
    }

    private ValueTask<MovementResult> NavigateToCenterOfHouse(CanvasTimingInformation context)
    {
        var diff = Maze.PixelCenterOfHouse - Maze.PixelHouseEntrancePoint;

        if (diff != Vector2.Zero)
        {
            diff = diff.Normalize();
            Ghost.Position += diff;
        }

        if (Ghost.Position.Round() == Maze.PixelCenterOfHouse)
        {
            _currentAction = NavigateToGhostIndexInHouse;
        }

        return new(MovementResult.NotFinished);
    }

    private ValueTask<MovementResult> NavigateToGhostIndexInHouse(CanvasTimingInformation context)
    {
        var diff = _ghostPosInHouse - Maze.PixelCenterOfHouse;

        if (diff != Vector2.Zero)
        {
            diff = diff.Normalize();
            Ghost.Position += diff;
        }

        if (Ghost.Position.Round() == _ghostPosInHouse)
        {
            Ghost.Direction = new(Direction.Down, Direction.Down);
            Ghost.SetMovementMode(GhostMovementMode.InHouse);
            return new(MovementResult.Finished);
        }

        return new(MovementResult.NotFinished);
    }

    public async override ValueTask<MovementResult> Update(CanvasTimingInformation context) => await _currentAction(context);

    private bool IsNearHouseEntrance() => Vector2s.AreNear(Ghost.Position, Maze.PixelHouseEntrancePoint, .75);
}