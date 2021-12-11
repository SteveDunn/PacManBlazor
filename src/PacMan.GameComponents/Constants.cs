namespace PacMan.GameComponents;

public static class Constants
{
    public static int FramesPerSecond { get; set; } = 60;

    public static readonly Vector2 UnscaledCanvasSize = new(224, 314);

    public static readonly float PacManBaseSpeed = 1.2f;
    public static readonly float GhostBaseSpeed = 1.2f;

#if DEBUG
    public static readonly int PacManLives = 3;
#else
        public static readonly int PacManLives = 3;
#endif
}