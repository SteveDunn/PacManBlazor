using System.Drawing;
using System.Numerics;

namespace PacMan.GameComponents
{

    // ReSharper disable once InconsistentNaming
    public static class Vector2s
    {
        public static Vector2 Zero = new Vector2(0, 0);
        public static Vector2 Two = new Vector2(2, 2);
        public static Vector2 Four = new Vector2(4, 4);
        public static Vector2 Eight = new Vector2(8, 8);

        public static bool AreNear(Vector2 first, Vector2 second, double range)
        {
            var r1 = new Rectangle((int) first.X, (int) first.Y, 1, 1);
            r1 = r1.Expand(range);

            var r2 = new Rectangle((int) second.X, (int) second.Y, 1, 1);
            r2 = r2.Expand(range);

            return r1.Intersects(r2);
        }

        public static Vector2 Lerp(Vector2 value1, Vector2 value2, float amount) => new Vector2(
            MathHelper.Lerp(value1.X, value2.X, amount),
            MathHelper.Lerp(value1.Y, value2.Y, amount));
    }
}