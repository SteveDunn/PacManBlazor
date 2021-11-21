using System.Numerics;
using System.Threading.Tasks;
using PacMan.GameComponents.Canvas;

namespace PacMan.GameComponents;

public interface IPacMan
{
    bool Visible { get; set; }

    Tile Tile { get; }

    Direction Direction { get; }

    Vector2 Position { get; }

    ValueTask Draw(CanvasWrapper session);

    void StartDying();

    void StartDigesting();

    ValueTask Update(CanvasTimingInformation timing);

    void PillEaten();

    ValueTask HandlePlayerStarting(PlayerStats playerStats, bool isDemo);
}