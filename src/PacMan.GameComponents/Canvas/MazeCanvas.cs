using System.Drawing;
using System.Threading.Tasks;
using Blazor.Extensions.Canvas.Canvas2D;

namespace PacMan.GameComponents.Canvas
{
    /// <summary>
    /// Represents the maze canvas.  Each Player has their own canvas and when they eat pills
    /// etc., their canvas has individual cells cleared.  When the 'main' maze is drawn,
    /// the current player's canvas is drawn.
    /// </summary>
    public class MazeCanvas : CanvasWrapper
    {
        public MazeCanvas(Canvas2DContext context) : base(context)
        {
        }

        public async ValueTask Reset()
        {
            int width = Spritesheet.Size.Width;
            int height = Spritesheet.Size.Height;

            var dim = Constants.UnscaledCanvasSize;

            await Clear((int)dim.X, (int)dim.Y);

            await DrawImage(
                Spritesheet.Reference,
                new Point(0, 0),
                new(0, 0, width, height));
        }
    }
}