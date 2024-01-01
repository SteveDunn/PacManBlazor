namespace PacMan.GameComponents;

public class DirectionChoices
{
    readonly Dictionary<Direction, bool> _lookup;

    public DirectionChoices()
    {
        _lookup = new();
    }

    public int Possibilities { get; private set; }

    public void Set(Direction direction)
    {
        _lookup[direction] = true;

        Possibilities = CalcPossibilities();
    }

    public void Unset(Direction direction)
    {
        _lookup[direction] = false;

        Possibilities = CalcPossibilities();
    }

    public bool IsSet(Direction direction) => _lookup[direction];

    public void ClearAll()
    {
        _lookup[Direction.Up] = false;
        _lookup[Direction.Down] = false;
        _lookup[Direction.Left] = false;
        _lookup[Direction.Right] = false;
    }

    int CalcPossibilities()
    {
        int count = 0;

        if (_lookup[Direction.Up])
        {
            ++count;
        }

        if (_lookup[Direction.Down])
        {
            ++count;
        }

        if (_lookup[Direction.Left])
        {
            ++count;
        }

        if (_lookup[Direction.Right])
        {
            ++count;
        }

        return count;
    }
}