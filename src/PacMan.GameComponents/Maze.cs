using System.Drawing;

namespace PacMan.GameComponents;

public class Maze : ISprite, IMaze
{
    // the point where the ghost goes to just before going into chase/scatter mode
    public readonly static Vector2 TileHouseEntrance = Tile.FromCell(13.5f, 11);

    public readonly static Vector2 PixelHouseEntrancePoint = Tile.ToCenterCanvas(new(13.5f, 11));

    // the point where the ghost goes to before going up and out of the house
    public readonly static Vector2 PixelCenterOfHouse = Tile.ToCenterCanvas(new(13.5f, 14));

    private readonly static CellIndex[] _specialIntersections =
    [
        new(12, 11),
        new(15, 11),
        new(12, 26),
        new(15, 26)
    ];

    private readonly static CellIndex[] _powerPillPositions =
    [
        new(1, 3),
        new(26, 3),
        new(1, 23),
        new(26, 23)
    ];

    private readonly LoopingTimer _timer;

    private readonly DirectionChoices _directionChoices = new();
    private readonly PowerPill _powerPill;
    private readonly GeneralSprite _whiteMazeCanvas;

    private bool _tickTock = true;

    private bool _flashing;

    private readonly static Size _spritesheetSize = new(225, 248);
    private readonly static Rectangle _mazeRect = new(0, 0, 225, 248);

    private readonly static Direction[] _directions =
    [
        Direction.Left,
        Direction.Right,
        Direction.Up,
        Direction.Down
    ];

    private MazeCanvas? _currentPlayerCanvas;
    private LevelStats? _levelStats;

    [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
    public Maze()
    {
        Visible = true;
        Origin = Vector2.Zero;
        _powerPill = new();
        _timer = new(250.Milliseconds(), () => _tickTock = !_tickTock);

        _whiteMazeCanvas = new(
            Vector2.Zero,
            _spritesheetSize,
            Vector2.Zero, new(228, 0));
    }

    public Vector2 SpriteSheetPos => Vector2.Zero;

    public bool Visible { get; }

    public Size Size => _spritesheetSize;

    // special intersections have an extra restriction
    // ghosts can not choose to turn upwards from these tiles.
    public static bool IsSpecialIntersection(CellIndex cell) =>
        cell == _specialIntersections[0] ||
        cell == _specialIntersections[1] ||
        cell == _specialIntersections[2] ||
        cell == _specialIntersections[3];

    public Vector2 Position => Vector2.Zero;

    public ValueTask Update(CanvasTimingInformation timing)
    {
        _timer.Run(timing);
        _powerPill.Update(timing);
        return default;
    }

    public async ValueTask Draw(CanvasWrapper session)
    {
        EnsureCanvas();
        if (_flashing)
        {
            if (_tickTock)
            {
                await session.DrawSprite(_whiteMazeCanvas, Spritesheet.Reference);
            }
            else
            {
                await session.DrawFromOther(_currentPlayerCanvas, new(0, 0), _mazeRect);
            }

            return;
        }

        await session.DrawFromOther(_currentPlayerCanvas, new(0, 0), _mazeRect);

        await DrawPowerPills(session);

        // this.drawGrid(8, 8, canvas);
    }

    [MemberNotNull(nameof(_currentPlayerCanvas))]
    private void EnsureCanvas()
    {
        if (_currentPlayerCanvas == null)
        {
            throw new InvalidOperationException("no current player canvas");
        }
    }

    public Vector2 Origin { get; }

    private async ValueTask DrawPowerPills(CanvasWrapper session)
    {
        EnsureLevelStats();

        foreach (var p in _powerPillPositions)
        {
            if (_levelStats.GetCellContent(p) == '*')
            {
                _powerPill.Position = p.ToVector2() * Vector2s.Eight;// - Vector2s.Four;

                await session.DrawSprite(_powerPill, Spritesheet.Reference);
            }
        }
    }

    [MemberNotNull(nameof(_levelStats))]
    private void EnsureLevelStats()
    {
        if (_levelStats == null)
        {
            throw new InvalidOperationException("no level stats");
        }
    }

    public async ValueTask ClearCell(CellIndex cell)
    {
        EnsureCanvas();
        var topLeft = Tile.FromIndex(cell).TopLeft;

        // CanvasBitmap bitmap = _bitmapsForPlayers[_gameStats.CurrentPlayerStats.PlayerIndex];

        // await bitmap.BeginBatch();

        await _currentPlayerCanvas.Clear((int) topLeft.X, (int) topLeft.Y, 8, 8);

        // await bitmap.FillRect((int)topLeft.X, (int)topLeft.Y, 8, 8, Color.Black);
        // await bitmap.EndBatch();
    }

    public static bool IsInTunnel(CellIndex point)
    {
        if (point.Y != 14)
        {
            return false;
        }

        if (point.X <= 5)
        {
            return true;
        }

        if (point.X >= 22)
        {
            return true;
        }

        return false;
    }

    public bool CanContinueInDirection(Direction direction, Tile tile)
    {
        var nextTile = tile.NextTile(direction);

        return !IsAWall(nextTile);
    }

    public DirectionChoices GetChoicesAtCellPosition(CellIndex cellPos)
    {
        _directionChoices.ClearAll();

        var tile = Tile.FromIndex(cellPos);

        foreach (var eachDirection in _directions)
        {
            if (!IsAWall(tile.NextTileWrapped(eachDirection)))
            {
                _directionChoices.Set(eachDirection);
            }
        }

        return _directionChoices;
    }

    private bool IsAWall(Tile tile) => GetTileContent(tile) == TileContent.Wall;

    public void StartFlashing() => _flashing = true;

    public void StopFlashing() => _flashing = false;

    public TileContent GetTileContent(Tile cell)
    {
        EnsureLevelStats();
        var a = _levelStats.GetCellContent(cell.Index);

        if (a == ' ')
        {
            return TileContent.Wall;
        }

        if (a == 'o')
        {
            return TileContent.Pill;
        }

        if (a == '*')
        {
            return TileContent.PowerPill;
        }

        if (a == '+')
        {
            return TileContent.Nothing;
        }

        return TileContent.Nothing;
    }

    // todo: use clamp
    public static CellIndex ConstrainCell(CellIndex cell)
    {
        var x = cell.X;
        var y = cell.Y;

        x = x < 0 ? 0 : x;
        x = x > MazeBounds.Dimensions.Width ? MazeBounds.Dimensions.Width : x;

        y = y < 0 ? 0 : y;
        y = y > MazeBounds.Dimensions.Height ? MazeBounds.Dimensions.Height : y;

        return new(x, y);
    }

    public ValueTask HandlePlayerStarted(PlayerStats playerStats, MazeCanvas playerCanvas)
    {
        _levelStats = playerStats.LevelStats;
        _currentPlayerCanvas = playerCanvas;

        return default;
    }
}