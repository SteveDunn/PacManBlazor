namespace PacMan.GameComponents;

public class FramePointers
{
    public FramePointers(
        Vector2 frame1,
        Vector2 frame2)
    {
        Frame1 = frame1;
        Frame2 = frame2;
    }

    public Vector2 Frame1 { get; }

    public Vector2 Frame2 { get; }
}