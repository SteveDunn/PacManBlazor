namespace PacMan.GameComponents;

public interface IStatusPanel
{
    void Update(CanvasTimingInformation timing);

    ValueTask Draw(CanvasWrapper ds);
}