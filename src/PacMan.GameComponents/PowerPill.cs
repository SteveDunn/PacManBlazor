namespace PacMan.GameComponents;

public class PowerPill : GeneralSprite
{
    public PowerPill() : base(
        Vector2s.Zero,
        Sizes.Eight,
        Vector2s.Zero,
        new(457, 156),
        new(467, 156),
        130.Milliseconds())
    {
    }
}