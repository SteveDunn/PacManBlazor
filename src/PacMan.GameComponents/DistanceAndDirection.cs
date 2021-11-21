using JetBrains.Annotations;

namespace PacMan.GameComponents;

[CannotApplyEqualityOperator]
public struct DistanceAndDirection
{
    public DistanceAndDirection(
        float distance,
        Direction direction)
    {
        Distance = distance;
        Direction = direction;
    }

    public float Distance { get; }

    public Direction Direction { get; }
}