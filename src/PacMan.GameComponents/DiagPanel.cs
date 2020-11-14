// ReSharper disable HeapView.BoxingAllocation

using System.Drawing;
using System.Threading.Tasks;
using PacMan.GameComponents.Canvas;

namespace PacMan.GameComponents
{
    public class DiagPanel
    {
        public async ValueTask Draw(CanvasWrapper ds)
        {
            await ds.SetGlobalAlphaAsync(.5f);

            await ds.FillRect(0, 0, 200, 100, Color.DarkSlateGray);

            await ds.DrawText($"FPS:{DiagInfo.Fps}", Point.Empty, Color.White);
            await ds.DrawText($"Tot time:{DiagInfo.TotalTime:c}", new Point(50, 00), Color.White);
            await ds.DrawText($"Draw count:{DiagInfo.DrawCount:D}", new Point(0, 15), Color.White);

            await ds.DrawText(
                $"Update count:{DiagInfo.UpdateCount:D} ({DiagInfo.UpdateCount - DiagInfo.DrawCount:D} more)",
                new Point(0, 30),
                Color.White);

            await ds.DrawText($"Elapsed:{DiagInfo.Elapsed:g}", new Point(0, 45), Color.White);

            await ds.DrawText($"loop tm taken:{DiagInfo.GameLoopDurationMs:D}", new Point(0, 60), Color.White);
            await ds.DrawText($"max loop tm taken:{DiagInfo.MaxGameLoopDurationMs:D}", new Point(0, 65), Color.White);

            await ds.DrawText($"slow frames:{DiagInfo.SlowElapsedCount:D}", new Point(0, 80), Color.White);

            await ds.SetGlobalAlphaAsync(1f);
        }
    }
}