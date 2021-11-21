using System.Numerics;

namespace PacMan.GameComponents;

public class Pill : GeneralSprite
{
    public Pill() : base(Vector2.Zero, new(8, 8), Vector2s.Four, new(8, 8))
    {
    }
}