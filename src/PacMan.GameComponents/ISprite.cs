using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using PacMan.GameComponents.Canvas;

namespace PacMan.GameComponents;

public interface ISprite
{
    public Vector2 Position { get; }

    public ValueTask Draw(CanvasWrapper session);

    public Vector2 Origin { get; }

    public Size Size { get; }

    public Vector2 SpriteSheetPos { get; }

    public bool Visible { get; }
}