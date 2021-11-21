using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using PacMan.GameComponents.Canvas;
using PacMan.GameComponents.Ghosts;

namespace PacMan.GameComponents;

public interface IFruit
{
    Vector2 Position { get; set; }

    Vector2 Origin { get; }

    Size Size { get; }

    Vector2 SpriteSheetPos { get; }

    bool Visible { get; }

    ValueTask Update(CanvasTimingInformation timing);

    ValueTask Draw(CanvasWrapper session);

    void SetFruitItem(FruitItem item);

    void HandlePlayerStarted(PlayerStats playerStats, bool isDemo);
}