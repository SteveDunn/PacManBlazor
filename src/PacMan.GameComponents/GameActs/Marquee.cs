using System.Diagnostics;
using System.Drawing;
using PacMan.GameComponents.Tweening;

namespace PacMan.GameComponents.GameActs;

public class Marquee
{
    private enum State
    {
        Idle,
        ScrollingIn,
        Stationary,
        ScrollingOut
    }

    private readonly MarqueeText[] _texts;
    private MarqueeText _current;
    private int _index;
    private State _state;
    private EggTimer _timer;
    private Vector2 _pos;
    private readonly TweeningFunction _tweeningFunction;
    private Tweener? _tweener;
    private readonly Tweener _colorTweener;
    private Color _color;

    public Marquee(MarqueeText[] texts)
    {
        _timer = new(1000.Seconds());
        _texts = texts;
        _index = -1;
        SelectNext();

        _tweeningFunction = Tweener.CreateTweeningFunction<Elastic>(Easing.EaseInOut);

        var colorTweeningFunction = Tweener.CreateTweeningFunction<Linear>(Easing.EaseNone);

        _colorTweener = new(.33f, 1, .33f.Seconds(), colorTweeningFunction);
        _colorTweener.Ended += () =>
        {
            _colorTweener.Reverse();
            _colorTweener.Reset();
        };
    }

    private void SelectNext()
    {
        if (++_index >= _texts.Length)
        {
            _index = 0;
        }

        _current = _texts[_index];
        _state = State.Idle;
        _timer = new(_current.TimeIdle);
        _pos = new(300, _current.YPosition);
    }

    public async ValueTask Update(CanvasTimingInformation timing)
    {
        _colorTweener.Update(timing);

        var col = (int)(_colorTweener.Position * 255);
        _color = Color.FromArgb(255, col, col, col);

        _tweener?.Update(timing);

        _timer.Run(timing);

        if (_state == State.Idle)
        {
            await Idle();
        }

        if (_state == State.ScrollingIn)
        {
            await ScrollingIn();
        }

        if (_state == State.Stationary)
        {
            await Stationary();
        }

        if (_state == State.ScrollingOut)
        {
            await ScrollingOut();
        }
    }

    private ValueTask ScrollingIn()
    {
        Debug.Assert(_tweener != null, $"{nameof(_tweener)} != null");

        _pos = new(_tweener.Position, _current.YPosition);

        if (_tweener.HasEnded)
        {
            _state = State.Stationary;
            _timer = new(_current.TimeStationary);
        }

        return default;
    }

    private ValueTask ScrollingOut()
    {
        Debug.Assert(_tweener != null, $"{nameof(_tweener)} != null");

        _pos = new(_tweener.Position, _current.YPosition);

        if (_tweener.HasEnded)
        {
            _state = State.Idle;
            _timer = new(_current.TimeIdle);
            SelectNext();
        }

        return default;
    }

    private ValueTask Idle()
    {
        if (_timer.Finished)
        {
            _state = State.ScrollingIn;
            _tweener = new(300, 0, _current.TimeIn, _tweeningFunction);
            _timer = new(_current.TimeOut);
        }

        return default;
    }

    private ValueTask Stationary()
    {
        if (_timer.Finished)
        {
            _state = State.ScrollingOut;
            _tweener = new(0, -300, _current.TimeOut, _tweeningFunction);
        }

        return default;
    }

    public async ValueTask Draw(CanvasWrapper session)
    {
        await session.DrawText(_current.Text, _pos.ToPoint(), _color);
    }
}