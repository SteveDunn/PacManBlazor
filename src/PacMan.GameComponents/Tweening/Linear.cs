namespace PacMan.GameComponents.Tweening;

[PublicAPI]
public class Linear
{
    public static float EaseNone(float t, float b, float c, float d)
    {
        return (c * t / d) + b;
    }

    public static float EaseIn(float t, float b, float c, float d)
    {
        return (c * t / d) + b;
    }

    public static float EaseOut(float t, float b, float c, float d)
    {
        return (c * t / d) + b;
    }

    public static float EaseInOut(float t, float b, float c, float d)
    {
        return (c * t / d) + b;
    }
}