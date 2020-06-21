using System.Numerics;
using JetBrains.Annotations;

namespace PacMan.GameComponents
{
    public class StartAndEndPos
    {
        public StartAndEndPos(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;
        }

        public Vector2 Start { get; }

        public Vector2 End { get; }

        [Pure]
        public StartAndEndPos Reverse() => new StartAndEndPos(End, Start);
    }
}