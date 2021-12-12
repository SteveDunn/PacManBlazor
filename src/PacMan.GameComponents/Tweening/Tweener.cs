
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace PacMan.GameComponents.Tweening;

public delegate float TweeningFunction(float timeElapsed, float start, float change, float duration);

public delegate void TweenEndHandler();

public class Tweener
{
    readonly TweeningFunction _tweeningFunction;
    float _from;
    readonly float _duration;

    bool _hasEnded;

    public event TweenEndHandler? Ended;

    public static TweeningFunction CreateTweeningFunction<T>(Easing easing)
    {
        return CreateTweeningFunction(typeof(T), easing);
    }

    public static TweeningFunction CreateTweeningFunction(Type type, Easing easing)
    {
        // ReSharper disable once HeapView.BoxingAllocation
        return (TweeningFunction)Delegate.CreateDelegate(typeof(TweeningFunction), type, easing.ToString());
    }

    public Tweener(float from, float to, float duration, TweeningFunction tweeningFunction)
    {
        _tweeningFunction = tweeningFunction;

        Running = true;
        _elapsed = 0.0f;
        _from = @from;
        _to = to;
        Position = @from;
        _change = to - @from;
        _duration = duration;
    }

    public Tweener(float from, float to, TimeSpan duration, TweeningFunction tweeningFunction)
        : this(from, to, (float)duration.TotalSeconds, tweeningFunction)
    {
    }

    public float Position
    {
        get;
        private set;
    }

    float _change;

    float _elapsed;
    float _to;

    public bool Running
    {
        get;
    }

    public void Update(CanvasTimingInformation gameTime)
    {
        if (!Running || (_elapsed == _duration))
        {
            return;
        }

        Position = _tweeningFunction(_elapsed, _from, _change, _duration);
        _elapsed += (float)gameTime.ElapsedTime.TotalSeconds;

        if (_elapsed >= _duration)
        {
            _elapsed = _duration;
            Position = _from + _change;
            onEnd();
        }
    }

    void onEnd()
    {
        _hasEnded = true;

        Ended?.Invoke();
    }

    public bool HasEnded => _hasEnded;

    public void Reset()
    {
        _hasEnded = false;
        _elapsed = 0.0f;
        _change = _to - _from;
        Position = _from;
    }

    public void Reverse()
    {
        var f = _from;
        _from = _to;
        _to = f;
        Position = f;
        _change = _to - _from;
        _elapsed = 0.0f;
    }

    [SuppressMessage("ReSharper", "HeapView.BoxingAllocation")]
    public override string ToString() =>
        $@"{_tweeningFunction.Method.DeclaringType?.Name}.{_tweeningFunction.Method.Name}. Tween {_from} -> {_from + _change} in {_duration}s. Elapsed {_elapsed:##0.##}s";
}