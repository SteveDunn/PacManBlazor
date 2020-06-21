using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using PacMan.GameComponents.Canvas;

namespace PacMan.GameComponents.Ghosts
{
    public interface IGhost
    {
        Tile Tile { get; }
        GhostNickname NickName { get; }
        DirectionInfo Direction { get; set; }
        bool Visible { get; set; }
        Size Size { get; }
        GhostState State { get; }
        bool IsInHouse { get; }
        Vector2 SpriteSheetPos { get; }
        Vector2 Origin { get; }
        void PowerPillEaten(GhostFrightSession session);
        void Reset();
        void StopMoving();
        ValueTask Update(CanvasTimingInformation timing);
        ValueTask Draw(CanvasWrapper session);
    }
}