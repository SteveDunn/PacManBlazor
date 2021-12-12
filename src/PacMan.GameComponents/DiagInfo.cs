namespace PacMan.GameComponents;

public static class DiagInfo
{
    static float _lastTimestamp;
    static TimeSpan _lastTimeKeyboardChecked;

    public static bool ShouldShow { get; private set; }

    public static int Fps
    {
        get;
        set;
    }

    public static TimeSpan TotalTime { get; private set; }

    public static ValueTask Update(CanvasTimingInformation info, IHumanInterfaceParser input)
    {
        TotalTime = info.TotalTime;

        if (info.TotalTime - _lastTimeKeyboardChecked > .25f.Seconds())
        {
            _lastTimeKeyboardChecked = info.TotalTime;

            if (input.IsKeyCurrentlyDown(Keys.D))
            {
                ShouldShow = !ShouldShow;
            }
        }

        if (input.WasKeyPressedAndReleased(Keys.A))
        {
            Constants.FramesPerSecond -= 5;
            Constants.FramesPerSecond = Math.Max(5, Constants.FramesPerSecond);
        }

        if (input.WasKeyPressedAndReleased(Keys.S))
        {
            Constants.FramesPerSecond += 5;
        }

        return default;
    }

    public static void IncrementUpdateCount() => ++UpdateCount;

    public static void IncrementDrawCount(float timestamp)
    {
        ++DrawCount;

        Elapsed = TimeSpan.FromMilliseconds(timestamp - _lastTimestamp);

        _lastTimestamp = timestamp;
    }

    public static int UpdateCount { get; private set; }

    public static int DrawCount { get; private set; }

    public static TimeSpan Elapsed { get; private set; }

    public static void UpdateTimeLoopTaken(in long elapsedMs)
    {
        if (elapsedMs > 17)
        {
            ++SlowElapsedCount;
        }

        GameLoopDurationMs = elapsedMs;

        MaxGameLoopDurationMs = Math.Max(MaxGameLoopDurationMs, GameLoopDurationMs);
    }

    public static int SlowElapsedCount { get; private set; }

    public static long GameLoopDurationMs { get; private set; }

    public static long MaxGameLoopDurationMs { get; private set; }
}