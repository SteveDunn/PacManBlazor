using System.Numerics;
using JetBrains.Annotations;

namespace PacMan.GameComponents.Ghosts
{
    [CannotApplyEqualityOperator]
    public struct FramePair
    {
        public FramePair(Vector2 first, Vector2 second)
        {
            First = first;
            Second = second;
        }

        public Vector2 First { get; }

        public Vector2 Second { get; }
    }
}