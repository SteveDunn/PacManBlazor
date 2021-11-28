using System.Drawing;
using System.Numerics;
using PacMan.GameComponents.Canvas;

namespace PacMan.GameComponents;

public class PacMan : ISprite, IPacMan
{
    readonly IMaze _maze;
    readonly IHumanInterfaceParser _input;
    readonly IMediator _mediator;
    public static readonly Vector2 FacingLeftSpritesheetPos = new(455, 16);

    CellIndex _lastDemoKeyPressAt = CellIndex.Zero;

    CellIndex _pillEatenAt = CellIndex.Zero;

    LifeStatus _lifeStatus;

    Vector2 _spriteSheetPos;

    Direction _direction;
    float _dyingFramePointer;

    Vector2 _frame1InSpriteMap;
    Vector2 _frame2InSpriteMap;
    TwoFrameAnimation _animDirection;

    float _speed = Constants.PacManBaseSpeed;

    bool _isDemoMode;

    readonly Dictionary<Direction, FramePointers> _framePointers;
    readonly List<Vector2> _dyingFrames;
    readonly DemoKeyPresses _demoKeyPresses;
    readonly Tile _tile;
    readonly KeyPressedEvent _keyPress;
    PlayerStats? _currentPlayerStats;

    public PacMan(IMaze maze, IHumanInterfaceParser input, IMediator mediator)
    {
        _maze = maze;
        _input = input;
        _mediator = mediator;
        _demoKeyPresses = new();

        _lifeStatus = LifeStatus.Alive;

        _tile = new();

        _keyPress = new();
        _framePointers = new();

        const int left = 456;
        const int left2 = 472;

        _framePointers[Direction.Up] = new(
            new(left, 32), new(left2, 32));

        _framePointers[Direction.Down] = new(
            new(left, 48), new(left2, 48));

        _framePointers[Direction.Left] = new(
            new(left, 16), new(left2, 16));

        _framePointers[Direction.Right] = new(
            new(left, 0), new(left2, 0));

        _dyingFrames = new();
        _dyingFramePointer = 0;

        for (var i = 0; i < 12; i++)
        {
            _dyingFrames.Add(new(489 + (i * 16), 0));
        }

        _animDirection = new(65.Milliseconds());

        Reset();
    }

    void resetAll(bool isDemoMode = false)
    {
        Visible = true;
        _demoKeyPresses.Reset();
        _isDemoMode = isDemoMode;
        _direction = Direction.Left;
        _speed = Constants.PacManBaseSpeed;
        _dyingFramePointer = 0;
        Position = Tile.ToCenterCanvas(new(13.5f, 23));
        _lifeStatus = LifeStatus.Alive;

        _animDirection = new(65.Milliseconds());
        _frame1InSpriteMap = _framePointers[_direction].Frame1;
        _frame2InSpriteMap = _framePointers[_direction].Frame2;

        _spriteSheetPos = _frame1InSpriteMap;
    }

    public bool Visible { get; set; }

    public Vector2 SpriteSheetPos => _spriteSheetPos;

    public Size Size { get; } = new(15, 15);

    public ValueTask Draw(CanvasWrapper session) => session.DrawSprite(this, Spritesheet.Reference);

    public Vector2 Origin => Vector2s.Eight;

    public Tile Tile => _tile;

    public void Reset()
    {
        resetAll();
    }

    public Direction Direction => _direction;

    void updateAnimation(CanvasTimingInformation args)
    {
        if (Math.Abs(_speed) < .0001)
        {
            return;
        }

        _animDirection.Run(args);
        _frame1InSpriteMap = _framePointers[_direction].Frame1;
        _frame2InSpriteMap = _framePointers[_direction].Frame2;

        _spriteSheetPos = _animDirection.Flag ? _frame1InSpriteMap : _frame2InSpriteMap;
    }

    void handleDying()
    {
        _dyingFramePointer += .15f;

        if (_dyingFramePointer > _dyingFrames.Count)
        {
            _lifeStatus = LifeStatus.Dead;
        }
        else
        {
            _spriteSheetPos = _dyingFrames[(int)Math.Floor(_dyingFramePointer)];
        }
    }

    public void StartDying()
    {
        _lifeStatus = LifeStatus.Dying;
    }

    public void StartDigesting()
    {
        _lifeStatus = LifeStatus.BeingDigested;
    }

    public async ValueTask Update(CanvasTimingInformation timing)
    {
        if (_currentPlayerStats == null)
        {
            throw new InvalidOperationException("no current player stats");
        }

        if (_lifeStatus == LifeStatus.BeingDigested)
        {
            return;
        }

        if (_lifeStatus is LifeStatus.Dying or LifeStatus.Dead)
        {
            handleDying();
            return;
        }

        updateAnimation(timing);

        if (_tile.IsNearCenter(2))
        {
            recordInput(timing);

            recenterInLane();

            handleDirection();
        }

        if (_tile.IsNearCenter(1.5))
        {
            await handleWhatIsUnderCell();

            var can = _maze.CanContinueInDirection(_direction, _tile);

            _speed = can ? Constants.PacManBaseSpeed : 0;
        }

        float speed = _speed;

        var levelProps = _currentPlayerStats.LevelStats.GetLevelProps();
        var inPillCell = _tile.Index == _pillEatenAt;

        var pcToUse = inPillCell ? levelProps.PacManDotsSpeedPc : levelProps.PacManSpeedPc;

        if (_currentPlayerStats.IsInFrightSession)
        {
            pcToUse = inPillCell ? levelProps.FrightPacManDotSpeedPc : levelProps.FrightPacManSpeedPc;
        }

        if (!inPillCell)
        {
            _pillEatenAt = CellIndex.Zero;
        }

        speed *= (pcToUse / 100);

        var offset = DirectionToIndexLookup.IndexVectorFor(_direction) * speed;

        Position += offset;
    }

    public Vector2 Position
    {
        get => _tile.SpritePos;
        private set => _tile.UpdateWithSpritePos(value);
    }

    public void PillEaten()
    {
        _pillEatenAt = _tile.Index;
    }

    void handleDirection()
    {
        if (_maze.CanContinueInDirection(_keyPress.Direction, _tile))
        {
            _direction = _keyPress.Direction;
        }
    }

    async ValueTask handleWhatIsUnderCell()
    {
        TileContent contents = _maze.GetTileContent(_tile);

        if (contents == TileContent.Pill)
        {
            await _maze.ClearCell(_tile.Index);

            await _mediator.Publish(new PillEatenEvent(_tile.Index));

            // _ = _game.PillEaten(_tile.Index);
        }

        if (contents == TileContent.PowerPill)
        {
            await _maze.ClearCell(_tile.Index);

            await _mediator.Publish(new PowerPillEatenEvent(_tile.Index));

            // _ = _game.PowerPillEaten(_tile.Index);
        }
    }

    void recordInput(CanvasTimingInformation context)
    {
        Direction requestedDirection = _direction;

        if (_isDemoMode)
        {
            if (_tile.IsNearCenter(4) && _tile.Index != _lastDemoKeyPressAt)
            {
                var choices = _maze.GetChoicesAtCellPosition(_tile.Index);

                choices.Unset(_direction);

                switch (_direction)
                {
                    case Direction.Left:
                        choices.Unset(Direction.Right);
                        break;
                    case Direction.Right:
                        choices.Unset(Direction.Left);
                        break;
                    case Direction.Up:
                        choices.Unset(Direction.Down);
                        break;
                    case Direction.Down:
                        choices.Unset(Direction.Up);
                        break;
                }

                if (choices.Possibilities >= 1)
                {
                    requestedDirection = _demoKeyPresses.Next();

                    _keyPress.When = context.TotalTime.TotalMilliseconds;

                    _keyPress.Direction = requestedDirection;

                    _lastDemoKeyPressAt = _tile.Index;
                }
            }
        }
        else
        {
            if (_input.IsRightKeyDown || _input.IsPanning(Keys.Right))
            {
                requestedDirection = Direction.Right;
            }
            else if (_input.IsLeftKeyDown || _input.IsPanning(Keys.Left))
            {
                requestedDirection = Direction.Left;
            }
            else if (_input.IsDownKeyDown || _input.IsPanning(Keys.Down))
            {
                requestedDirection = Direction.Down;
            }
            else if (_input.IsUpKeyDown || _input.IsPanning(Keys.Up))
            {
                requestedDirection = Direction.Up;
            }
        }

        _keyPress.Direction = requestedDirection;
        _keyPress.When = context.TotalTime.TotalMilliseconds;
    }

    // debt: SD: refactor into something common as this is also used for the ghosts
    void recenterInLane()
    {
        var tileCenter = _tile.CenterPos;

        var speed = _speed;

        if (_direction == Direction.Down || _direction == Direction.Up)
        {
            var wayToMove = new Vector2(speed, 0);

            if (Position.X > tileCenter.X)
            {
                Position -= wayToMove;
                Position = new(Math.Max(Position.X, tileCenter.X), Position.Y);
            }
            else if (Position.X < tileCenter.X)
            {
                Position += wayToMove;
                Position = new(Math.Min(Position.X, tileCenter.X), Position.Y);
            }
        }

        if (_direction == Direction.Left || _direction == Direction.Right)
        {
            var wayToMove = new Vector2(0, speed);

            if (Position.Y > tileCenter.Y)
            {
                Position -= wayToMove;
                Position = new(Position.X, Math.Max(Position.Y, tileCenter.Y));
            }
            else if (Position.Y < tileCenter.Y)
            {
                Position += wayToMove;
                Position = new(Position.X, Math.Min(Position.Y, tileCenter.Y));
            }
        }
    }

    public ValueTask HandlePlayerStarting(PlayerStats playerStats, bool isDemo)
    {
        _currentPlayerStats = playerStats;

        resetAll(isDemo);

        return default;
    }
}