using System.Drawing;

namespace PacMan.GameComponents.Ghosts;

public class Pinky : Ghost
{
    private readonly IMaze _maze;
    private readonly IPacMan _pacman;

    public Pinky(IGameStats gameStats, IMediator mediator, IMaze maze, IPacMan pacman, IHumanInterfaceParser input) : base(
        gameStats,
        mediator,
        input,
        pacman,
        GhostNickname.Pinky, maze, Tile.FromCell(15.5f, 11f), GameComponents.Direction.Down)
    {
        _maze = maze;
        _pacman = pacman;
        HouseOffset = 0;
    }

    public override Color GetColor() => Color.Pink;

    public override ValueTask<CellIndex> GetScatterTarget() => new(new CellIndex(2, 0));

    public override ValueTask<CellIndex> GetChaseTarget() => GetChaseTargetCell();

    public override void Reset()
    {
        base.Reset();

        Direction = new(GameComponents.Direction.Down, GameComponents.Direction.Down);

        State = GhostState.Normal;
        MovementMode = GhostMovementMode.InHouse;
        SetMover(new(this, _maze, CurrentPlayerStats.GhostHouseDoor));
    }

    // Pac-Man’s current position and orientation, and selecting the location four tiles straight
    // ahead of him. Works when PacMan is facing left, down, or right, but when facing upwards,
    // it's also four tiles to the left
    private ValueTask<CellIndex> GetChaseTargetCell()
    {
        var pacDir = _pacman.Direction;

        CellIndex pacTile = _pacman.Tile.Index;

        CellIndex offset = Maze.ConstrainCell(
            pacTile + (DirectionToIndexLookup.IndexVectorFor(pacDir) * Vector2s.Four).ToCellIndex());

        // for the bug in the original pacman
        if (pacDir == GameComponents.Direction.Up)
        {
            offset = (offset.ToVector2() + new Vector2(-4, 0)).ToCellIndex();
        }

        var newTarget = Maze.ConstrainCell(pacTile + offset);

        return new(newTarget);
    }

    // todo:
    //        draw(canvas: Canvas) : void {
    //            super.draw(canvas);
    //            if (Diags.enabled) {
    //                this.maze.highlightCell(canvas, this.getChaseTargetCell(), "pink");
    //            }
    //        };
}