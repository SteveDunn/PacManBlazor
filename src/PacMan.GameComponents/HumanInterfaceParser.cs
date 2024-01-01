namespace PacMan.GameComponents;

public static class Keys
{
    public const byte Up = 38;
    public const byte Down = 40;
    public const byte Left = 37;
    public const byte Right = 39;
    public const byte A = 65;
    public const byte D = 68;
    public const byte P = 80;
    public const byte S = 83;
    public const byte One = 49;
    public const byte Two = 50;
    public const byte Three = 51;
    public const byte Four = 52;
    public const byte Five = 53;
    public const byte Six = 54;
    public const byte Space = 32;
}

public class HumanInterfaceParser : IHumanInterfaceParser
{
    // ReSharper disable once HeapView.ObjectAllocation.Evident
    readonly bool[] _keysCurrentlyDown = new bool[256];

    readonly TimeSpan[] _keyPresses;
    readonly TimeSpan[] _swipes;

    TimeSpan _timeTapped;
    TimeSpan _timeLongPressed;

    public HumanInterfaceParser()
    {
        var oneSecondAgo = 1.Seconds();

        _keyPresses = Enumerable.Range(0, 256).Select(_ => -oneSecondAgo).ToArray();
        _swipes = Enumerable.Range(0, 256).Select(_ => -oneSecondAgo).ToArray();
        _timeLongPressed = -oneSecondAgo;
        _timeTapped = -oneSecondAgo;
        _totalGameTime = 0.Seconds();
    }

    TimeSpan _totalGameTime;

    // 20 milliseconds when 60 fps - should be 120 for 30 fps = e.g. fps * 2
    static TimeSpan GetRetention() => (60 / Constants.FramesPerSecond * 20).Milliseconds();

    public bool IsRightKeyDown => IsKeyCurrentlyDown(Keys.Right);

    public bool WasTapped => _totalGameTime - _timeTapped <= GetRetention();

    public bool WasLongPress => _totalGameTime - _timeLongPressed <= GetRetention();

    public bool IsLeftKeyDown => IsKeyCurrentlyDown(Keys.Left);

    public bool IsUpKeyDown => IsKeyCurrentlyDown(Keys.Up);

    public bool IsDownKeyDown => IsKeyCurrentlyDown(Keys.Down);

    public void Update(CanvasTimingInformation timing)
    {
        _totalGameTime = timing.TotalTime;
    }

    public bool IsKeyCurrentlyDown(byte key) => _keysCurrentlyDown[key];

    /// <summary>
    /// A 'press' is pressing and releasing, not just 'pushed'
    /// To see if a key is 'down', use 'IsKeyCurrentlyDown'
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool WasKeyPressedAndReleased(byte key)
    {
        TimeSpan whenLastPressed = _keyPresses[key];

        bool wasKeyPressed = _totalGameTime - whenLastPressed <= GetRetention();

        // Debug.WriteLine($"Total game time: {_totalGameTime}, keypress for {key}: {whenLastPressed} - key was pressed {_totalGameTime - whenLastPressed} ago.  Was key pressed? {wasKeyPressed}");

        return wasKeyPressed;
    }

    /// <summary>
    /// A 'press' is pressing and releasing, not just 'pushed'
    /// To see if a key is 'down', use 'IsKeyCurrentlyDown'
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool IsPanning(byte key)
    {
        TimeSpan whenLastPressed = _swipes[key];

        bool wasKeyPressed = _totalGameTime - whenLastPressed <= GetRetention();

        // Debug.WriteLine($"Total game time: {_totalGameTime}, keypress for {key}: {whenLastPressed} - key was pressed {_totalGameTime - whenLastPressed} ago.  Was key pressed? {wasKeyPressed}");

        return wasKeyPressed;
    }

    public void TapHappened() => _timeTapped = _totalGameTime;

    public void LongPressHappened() => _timeLongPressed = _totalGameTime;

    public void KeyDown(byte key) => _keysCurrentlyDown[key] = true;

    public void Swiped(byte key) => _swipes[key] = _totalGameTime;

    public void KeyUp(byte key)
    {
        _keysCurrentlyDown[key] = false;
        _keyPresses[key] = _totalGameTime;
    }
}