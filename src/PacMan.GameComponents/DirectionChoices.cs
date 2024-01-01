namespace PacMan.GameComponents;

public class DirectionChoices
{
    private readonly Dictionary<Direction, bool> _lookup = new();

    public bool AnyAvailable { get; private set; }

    public void Set(Direction direction)
    {
        _lookup[direction] = true;

        AnyAvailable = CalcPossibilities();
    }

    public void Unset(Direction direction)
    {
        _lookup[direction] = false;

        AnyAvailable = CalcPossibilities();
    }

    public bool CanGoUp => _lookup[Direction.Up];
    public bool CanGoDown => _lookup[Direction.Down];
    public bool CanGoLeft => _lookup[Direction.Left];
    public bool CanGoRight => _lookup[Direction.Right];

    public void ClearAll()
    {
        _lookup[Direction.Up] = false;
        _lookup[Direction.Down] = false;
        _lookup[Direction.Left] = false;
        _lookup[Direction.Right] = false;
    }

    private bool CalcPossibilities() => 
        CanGoUp || CanGoDown || CanGoLeft || CanGoRight;
}