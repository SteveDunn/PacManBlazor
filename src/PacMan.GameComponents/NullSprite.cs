using System.Numerics;

namespace PacMan.GameComponents
{
    public class NullSprite : GeneralSprite
    {
        public NullSprite() : base(Vector2.Zero, System.Drawing.Size.Empty, Vector2.Zero, Vector2.Zero)
        {
        }
    }
}