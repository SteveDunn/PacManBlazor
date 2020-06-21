using System.Numerics;

namespace PacMan.GameComponents
{
    public class PowerPill : GeneralSprite
    {
        public PowerPill() : base(
            Vector2s.Zero,
            Sizes.Eight,
            Vector2s.Zero,
            new Vector2(457, 156),
            new Vector2(467, 156),
            130.Milliseconds())
        {
        }
    }
}