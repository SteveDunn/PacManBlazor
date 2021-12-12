namespace PacMan.GameComponents.Ghosts;

public class GhostFrightSession
{
    readonly TimeSpan _eachFlashDurationMs = TimeSpan.FromMilliseconds(166);
    readonly LoopingTimer _timer;

    int _amountOfGhostsEaten;

    TimeSpan _timeLeft;

    // 1s = 1000ms
    // 1/60th second = 16.66ms
    // 1 frame = 16.66ms
    // each flash takes 1/6th of a second (or 10 frames), 166ms
    // so there can be 6 flashes a second
    TimeSpan _timeLeftToStartFlashing;
    bool _tickTock;

    public GhostFrightSession(LevelProps levelProps)
    {
        _amountOfGhostsEaten = 0;
        _timeLeft = levelProps.FrightGhostTime.Value;

        var flashesLeft = levelProps.FrightGhostFlashes;

        _timeLeftToStartFlashing = _timeLeft -
                                   TimeSpan.FromMilliseconds(flashesLeft * _eachFlashDurationMs.TotalMilliseconds);
        _timer = new(
            _eachFlashDurationMs, () => _tickTock = !_tickTock);
    }

    public void Update(CanvasTimingInformation timing)
    {
        var elapsed = timing.ElapsedTime;

        _timer.Run(timing);
        _timeLeft -= elapsed;
        _timeLeftToStartFlashing -= elapsed;
    }

    public bool IsWhite => _timeLeftToStartFlashing <= TimeSpan.Zero && _tickTock;

    // todo: cqs
    public Points GhostEaten()
    {
        ++_amountOfGhostsEaten;
        return Points.From((int) (Math.Pow(2, _amountOfGhostsEaten) * 100));
    }

    public bool IsFinished => _timeLeft <= TimeSpan.Zero;
}