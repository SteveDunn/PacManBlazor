using System;
using System.Numerics;
using JetBrains.Annotations;

namespace PacMan.GameComponents
{
    public struct CellIndex : IEquatable<CellIndex>
    {
        public CellIndex(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool IsInBounds =>
            X >= 0 && X < MazeBounds.Dimensions.Width && Y >= 0 && Y < MazeBounds.Dimensions.Height;

        public static bool operator ==(CellIndex left, CellIndex right) => left.Equals(right);

        public static bool operator !=(CellIndex left, CellIndex right) => !left.Equals(right);

        public static CellIndex Zero = new CellIndex(0, 0);

        public int X { get; }

        public int Y { get; }

        public bool Equals(CellIndex other) => X == other.X && Y == other.Y;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is CellIndex && Equals((CellIndex) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public static CellIndex operator +(CellIndex left, CellIndex right) => new CellIndex(left.X + right.X, left.Y + right.Y);

        public static CellIndex operator -(CellIndex left, CellIndex right) => new CellIndex(left.X - right.X, left.Y - right.Y);

        [Pure]
        public static CellIndex FromSpritePos( Vector2 spritePos)
        {
            Vector2 vector2 = spritePos / Vector2s.Eight;

            return new CellIndex((int) vector2.X, (int) vector2.Y);
        }

// #if DEBUG
//         public override string ToString() => $"{X}, {Y}, IsInBounds={IsInBounds}";
// #endif
    }
}