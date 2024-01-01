﻿using System.Drawing;

namespace PacMan.GameComponents.Ghosts;

public class Blinky : Ghost
{
    private readonly IPacMan _pacman;
    private readonly ValueTask<CellIndex> _scatterTarget;

    public Blinky(IGameStats gameStats, IMediator mediator, IMaze maze, IPacMan pacman, IHumanInterfaceParser input) : base(
        gameStats,
        mediator,
        input,
        pacman,
        GhostNickname.Blinky,
        maze,
        new(13.5f, 11),
        GameComponents.Direction.Left)
    {
        _pacman = pacman;
        HouseOffset = 0;
        _scatterTarget = new(new CellIndex(25, 0));
    }

    public override Color GetColor() => Color.Red;

    public override ValueTask<CellIndex> GetScatterTarget() => _scatterTarget;

    public override ValueTask<CellIndex> GetChaseTarget() => GetChaseTargetCell();

    public override void Reset()
    {
        base.Reset();

        State = GhostState.Normal;
        MovementMode = GhostMovementMode.Undecided;
    }

    // Whenever Clyde needs to determine his target tile, he first calculates his distance from Pac-Man.
    // If he is farther than eight tiles away, his targeting is identical to Blinky’s,
    // using Pac-Man’s current tile as his target. However, as soon as his distance
    // to Pac-Man becomes less than eight tiles, Clyde’s target is set to the same tile as his fixed
    // one in Scatter mode, just outside the bottom-left corner of the maze
    // Pac-Man’s current position and orientation, and selecting the location four tiles straight
    // ahead of him. Works when PacMan is facing left, down, or right, but when facing upwards,
    // it's also four tiles to the left
    private ValueTask<CellIndex> GetChaseTargetCell()
    {
        CellIndex pacCellPos = _pacman.Tile.Index;

        return new(pacCellPos);
    }

    // we are reading these properties:
    // elroy1DotsLeft
    // elroy1SpeedPc
    // elroy2DotsLeft
    // elroy2SpeedPc

    protected override SpeedPercentage GetNormalGhostSpeedPercent()
    {
        LevelStats levelStats = CurrentPlayerStats.LevelStats;

        LevelProps levelProps = levelStats.GetLevelProps();

        int pillsRemaining = levelStats.PillsRemaining;

        if (pillsRemaining > levelProps.Elroy1DotsLeft)
        {
            return levelProps.GhostSpeedPc;
        }

        if (pillsRemaining < levelProps.Elroy2DotsLeft)
        {
            return levelProps.Elroy2SpeedPc;
        }

        return levelProps.Elroy1SpeedPc;
    }
}