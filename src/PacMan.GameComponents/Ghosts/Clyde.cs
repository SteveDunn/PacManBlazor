using System.Drawing;

namespace PacMan.GameComponents.Ghosts;

public class Clyde : Ghost
{
    readonly IMaze _maze;
    readonly IPacMan _pacman;
    readonly ValueTask<CellIndex> _scatterTarget = new(new CellIndex(0, 29));

    public Clyde(IGameStats gameStats, IMediator mediator, IMaze maze, IPacMan pacman, IHumanInterfaceParser input) : base(
        gameStats,
        mediator,
        input,
        pacman,
        GhostNickname.Clyde, maze, new(11.5f, 12), GameComponents.Direction.Up)
    {
        _maze = maze;
        _pacman = pacman;

        HouseOffset = 1;
    }

    public override Color GetColor() => Color.YellowGreen;

    public override ValueTask<CellIndex> GetScatterTarget() => _scatterTarget;

    public override ValueTask<CellIndex> GetChaseTarget() => _getChaseTargetCell();

    public override void Reset()
    {
        base.Reset();

        Direction = new(GameComponents.Direction.Up, GameComponents.Direction.Up);

        State = GhostState.Normal;

        MovementMode = GhostMovementMode.InHouse;

        SetMover(new(this, _maze, CurrentPlayerStats.ghostHouseDoor));
    }

    // Whenever Clyde needs to determine his target tile, he first calculates his distance from Pac-Man.
    // If he is farther than eight tiles away, his targeting is identical to Blinky’s,
    // using Pac-Man’s current tile as his target. However, as soon as his distance
    // to Pac-Man becomes less than eight tiles, Clyde’s target is set to the same tile as his fixed
    // one in Scatter mode, just outside the bottom-left corner of the maze
    // Pac-Man’s current position and orientation, and selecting the location four tiles straight
    // ahead of him. Works when PacMan is facing left, down, or right, but when facing upwards,
    // it's also four tiles to the left
    ValueTask<CellIndex> _getChaseTargetCell()
    {
        CellIndex pacCellPos = _pacman.Tile.Index;

        CellIndex myPos = Tile.Index;

        var distance = Math.Abs(Extensions.DistanceBetween(myPos, pacCellPos));

        if (distance >= 8)
        {
            return new(pacCellPos);
        }

        return _scatterTarget;
    }

    // todo:
    // draw(canvas: Canvas) : void {
    //    super.draw(canvas);
    //
    //    if (Diags.enabled) {
    //        this.maze.highlightCell(canvas, this._getChaseTargetCell(), "orange");
    //    }
    // };
}