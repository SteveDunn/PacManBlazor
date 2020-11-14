using System.Collections.Generic;
using System.Numerics;

namespace PacMan.GameComponents
{
    public static class DirectionToIndexLookup
    {
        static readonly Dictionary<Directions, Vector2> _lookup;

        static DirectionToIndexLookup()
        {
            _lookup = new() {
                { Directions.Up, new(0, -1) },
                { Directions.Down, new(0, 1) },
                { Directions.Left, new(-1, 0) },
                { Directions.Right, new(1, 0) }
            };
        }

        public static Vector2 IndexVectorFor(Directions direction) => _lookup[direction];

        public static Directions GetDirectionFromVector(Vector2 vector)
        {
            var unitVector = new Vector2(vector.X, vector.Y).Normalize();

            if (unitVector.X < 0)
            {
                return Directions.Left;
            }

            if (unitVector.X > 0)
            {
                return Directions.Right;
            }

            if (unitVector.Y < 0)
            {
                return Directions.Up;
            }

            if (unitVector.Y > 0)
            {
                return Directions.Down;
            }

            return Directions.None;
        }
    }
}