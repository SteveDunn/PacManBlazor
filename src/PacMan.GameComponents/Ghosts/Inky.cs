using System.Drawing;

namespace PacMan.GameComponents.Ghosts;

public class Inky : Ghost
{
    readonly IMediator _mediator;
    readonly IMaze _maze;
    readonly IPacMan _pacman;
    readonly GetBlinkyRequest _getBlinkyRequest;

    public override Color GetColor() => Color.Aqua;

    public override ValueTask<CellIndex> GetScatterTarget() => new(new CellIndex(27, 29));

    public override ValueTask<CellIndex> GetChaseTarget() => GetChaseTargetCell();

    public Inky(IGameStats gameStats, IMediator mediator, IMaze maze, IPacMan pacman, IHumanInterfaceParser input) : base(
        gameStats,
        mediator,
        input,
        pacman,
        GhostNickname.Inky, maze, Tile.FromCell(15.5f, 11), GameComponents.Direction.Up)
    {
        _mediator = mediator;
        _maze = maze;
        _pacman = pacman;
        HouseOffset = -1;

        _getBlinkyRequest = new();
    }

    [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
    public override void Reset()
    {
        base.Reset();

        Direction = new(GameComponents.Direction.Up, GameComponents.Direction.Up);

        State = GhostState.Normal;
        MovementMode = GhostMovementMode.InHouse;
        SetMover(new(this, _maze, CurrentPlayerStats.GhostHouseDoor));
    }

    // To locate Inky’s target, we first start by selecting the position two tiles in front of Pac-Man
    // in his current direction of travel.
    // From there, imagine drawing a vector from Blinky’s position to this tile, and then doubling
    // the length of the vector. The tile that this new, extended vector ends on will be Inky’s actual target
    async ValueTask<CellIndex> GetChaseTargetCell()
    {
        // ReSharper disable once HeapView.BoxingAllocation
        var blinky = await _mediator.Send(_getBlinkyRequest);

        var blinkyCell = blinky.Tile.Index;

        var pacDir = _pacman.Direction;

        CellIndex pacCellPos = _pacman.Tile.Index;

        var twoCellsInFront = Maze.ConstrainCell(
            pacCellPos + (DirectionToIndexLookup.IndexVectorFor(pacDir) * Vector2s.Two).ToCellIndex());

        CellIndex diff = twoCellsInFront - blinkyCell;

        var diff2 = (diff.ToVector2() * Vector2s.Two).ToCellIndex();

        var newTarget = Maze.ConstrainCell(blinkyCell + diff2);

        return newTarget;
    }

    // todo:
    //    draw(canvas: Canvas) : void {
    //    super.draw(canvas);
    //    if (Diags.enabled) {
    //        this.maze.highlightCell(canvas, this.getChaseTargetCell(), "aqua");
    //    }
}