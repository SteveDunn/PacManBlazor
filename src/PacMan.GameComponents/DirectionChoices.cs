using System.Collections.Generic;

namespace PacMan.GameComponents
{
    public class DirectionChoices
    {
        readonly Dictionary<Directions, bool> _lookup;

        public DirectionChoices()
        {
            _lookup = new Dictionary<Directions, bool>();
        }

        public int Possibilities { get; private set; }

        public void Set(Directions direction)
        {
            _lookup[direction] = true;

            Possibilities = calcPossibilities();
        }

        public void Unset(Directions direction)
        {
            _lookup[direction] = false;

            Possibilities = calcPossibilities();
        }

        public bool IsSet(Directions direction) => _lookup[direction];

        public void ClearAll()
        {
            _lookup[Directions.Up] = false;
            _lookup[Directions.Down] = false;
            _lookup[Directions.Left] = false;
            _lookup[Directions.Right] = false;
        }

        int calcPossibilities()
        {
            int count = 0;

            if (_lookup[Directions.Up])
            {
                ++count;
            }

            if (_lookup[Directions.Down])
            {
                ++count;
            }

            if (_lookup[Directions.Left])
            {
                ++count;
            }

            if (_lookup[Directions.Right])
            {
                ++count;
            }

            return count;
        }
    }
}