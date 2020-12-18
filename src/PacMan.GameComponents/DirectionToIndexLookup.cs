using System.Collections.Generic;
using System.Numerics;

namespace PacMan.GameComponents
{
    public static class DirectionToIndexLookup
    {
        static readonly Dictionary<Direction, Vector2> _lookup;

        static DirectionToIndexLookup()
        {
            _lookup = new() {
                { Direction.Up, new(0, -1) },
                { Direction.Down, new(0, 1) },
                { Direction.Left, new(-1, 0) },
                { Direction.Right, new(1, 0) }
            };
        }

        public static Vector2 IndexVectorFor(Direction direction) => _lookup[direction];

        public static Direction GetDirectionFromVector(Vector2 vector)
        {
            var unitVector = new Vector2(vector.X, vector.Y).Normalize();

            if (unitVector.X < 0)
            {
                return Direction.Left;
            }

            if (unitVector.X > 0)
            {
                return Direction.Right;
            }

            if (unitVector.Y < 0)
            {
                return Direction.Up;
            }

            if (unitVector.Y > 0)
            {
                return Direction.Down;
            }

            return Direction.None;
        }
    }
}