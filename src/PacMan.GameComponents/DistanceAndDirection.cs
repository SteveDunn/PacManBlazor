using JetBrains.Annotations;

namespace PacMan.GameComponents
{
    [CannotApplyEqualityOperator]
    public struct DistanceAndDirection
    {
        public DistanceAndDirection(
            float distance,
            Directions direction)
        {
            Distance = distance;
            Direction = direction;
        }

        public float Distance { get; }

        public Directions Direction { get; }
    }
}