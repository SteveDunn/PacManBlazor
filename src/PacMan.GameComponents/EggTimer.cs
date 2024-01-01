namespace PacMan.GameComponents;

public class EggTimer
{
    private readonly Action _whenFinished;
    private readonly TimeSpan _duration;

    private bool _isPaused;
    private bool _isFinished;
    private TimeSpan _currentTime;

    private static void DoNothing()
    {
    }

    public EggTimer(TimeSpan duration, Action whenFinished) : this(duration) =>
        _whenFinished = whenFinished;

    public EggTimer(TimeSpan duration)
    {
        _duration = duration;
        _currentTime = duration;
        _whenFinished = DoNothing;
    }

    public void Reset() => _currentTime = _duration;

    private double GetPercentProgress()
    {
        TimeSpan msGone = _duration - _currentTime;

        double pc = msGone.TotalMilliseconds * 100f / _duration.TotalMilliseconds;

        return pc;
    }

    public float Progress => (float)(GetPercentProgress() / 100f);

    public bool Finished => _isFinished;

    public static EggTimer Unset => new(TimeSpan.MaxValue);

    public void Run(CanvasTimingInformation timing)
    {
        if (_isFinished)
        {
            return;
        }

        if (_isPaused)
        {
            return;
        }

        _currentTime -= timing.ElapsedTime;

        if (_currentTime < TimeSpan.Zero)
        {
            _isFinished = true;
            _whenFinished();
        }
    }

    public void Pause() => _isPaused = true;

    public void Resume() => _isPaused = false;
}