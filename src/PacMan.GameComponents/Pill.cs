using System.Drawing;
using System.Numerics;

namespace PacMan.GameComponents
{
    public class Pill : GeneralSprite
    {
        public Pill() : base(Vector2.Zero, new Size(8, 8), Vector2s.Four, new Vector2(8, 8))
        {
        }
    }
}