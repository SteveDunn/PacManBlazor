using System.Threading.Tasks;
using PacMan.GameComponents.Canvas;
using PacMan.GameComponents.Ghosts;

namespace PacMan.GameComponents
{
    public interface IGhostCollection
    {
        IGhost GetGhost(GhostNickname nickName);
        
        IGhost[] Ghosts { get; }

        ValueTask DrawAll(CanvasWrapper canvas);
        
        ValueTask Update(CanvasTimingInformation timing);
    }
}