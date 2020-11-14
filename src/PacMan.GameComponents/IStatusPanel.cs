using System.Threading.Tasks;
using PacMan.GameComponents.Canvas;

namespace PacMan.GameComponents
{
    public interface IStatusPanel
    {
        void Update(CanvasTimingInformation timing);

        ValueTask Draw(CanvasWrapper ds);
    }
}