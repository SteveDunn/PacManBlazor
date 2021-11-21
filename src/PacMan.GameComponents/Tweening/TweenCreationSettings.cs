using System;

namespace PacMan.GameComponents.Tweening;

public class TweenCreationSettings
{
    public TweenCreationSettings(Type type, Easing easing, TimeSpan duration)
    {
        Type = type;
        Easing = easing;
        Duration = duration;
    }

    public Type Type
    {
        get;
        private set;
    }

    public Easing Easing
    {
        get;
        private set;
    }

    public TimeSpan Duration
    {
        get;
        private set;
    }

    public TweenCreationSettings(Type type, Easing easing)
    {
        Type = type;
        Easing = easing;
    }

    public static TweenCreationSettings Create<T>()
    {
        return Create<T>(Easing.EaseInOut);
    }

    public static TweenCreationSettings Create<T>(Easing easing)
    {
        return new(typeof(T), easing);
    }
}