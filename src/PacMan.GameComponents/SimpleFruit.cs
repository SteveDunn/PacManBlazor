using System.Drawing;
using PacMan.GameComponents.Ghosts;

namespace PacMan.GameComponents;

public class SimpleFruit : ISprite
{
    public SimpleFruit()
    {
        Visible = true;
        Position = Vector2.Zero;

        SetFruitItem(FruitItem.Apple);
    }

    public Vector2 Position { get; set; }

    public ValueTask Draw(CanvasWrapper session)
    {
        return session.DrawSprite(this, Spritesheet.Reference);
    }

    public Vector2 Origin => Vector2s.Eight;

    public Size Size => new(14, 14);

    public Vector2 SpriteSheetPos { get; private set; }

    public bool Visible { get; protected set; }

    public void SetFruitItem(FruitItem item)
    {
        int x = 16 * (int)item;

        SpriteSheetPos = new(490 + x, 50);
    }
}