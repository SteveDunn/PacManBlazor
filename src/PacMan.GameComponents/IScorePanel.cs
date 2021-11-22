using PacMan.GameComponents.Canvas;

namespace PacMan.GameComponents;

public interface IScorePanel
{
    void Update(CanvasTimingInformation timingInformation);

    ValueTask Draw(CanvasWrapper ds);
}