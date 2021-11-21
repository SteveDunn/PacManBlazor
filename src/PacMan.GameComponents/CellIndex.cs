using System;
using System.Numerics;
using JetBrains.Annotations;

namespace PacMan.GameComponents;

public readonly struct CellIndex : IEquatable<CellIndex>
{
    public CellIndex(int x, int y) => (X, Y) = (x, y);

    public bool IsInBounds =>
        X >= 0 && X < MazeBounds.Dimensions.Width && Y >= 0 && Y < MazeBounds.Dimensions.Height;

    public static bool operator ==(in CellIndex left, in CellIndex right) => left.Equals(right);

    public static bool operator !=(in CellIndex left, in CellIndex right) => !left.Equals(right);

    public static readonly CellIndex Zero = new(0, 0);

    public int X { get; }

    public int Y { get; }

    public bool Equals(CellIndex other) => X == other.X && Y == other.Y;

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        return obj is CellIndex index && Equals(index);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (X * 397) ^ Y;
        }
    }

    public static CellIndex operator +(in CellIndex left, in CellIndex right) => new(left.X + right.X, left.Y + right.Y);

    public static CellIndex operator -(in CellIndex left, in CellIndex right) => new(left.X - right.X, left.Y - right.Y);

    [Pure]
    public static CellIndex FromSpritePos(Vector2 spritePos)
    {
        (int x, int y) = spritePos / Vector2s.Eight;

        return new(x,y);
    }

    // #if DEBUG
    //         public override string ToString() => $"{X}, {Y}, IsInBounds={IsInBounds}";
    // #endif
}