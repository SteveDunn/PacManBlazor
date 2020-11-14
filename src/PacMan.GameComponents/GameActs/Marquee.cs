using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using PacMan.GameComponents.Canvas;
using PacMan.GameComponents.Tweening;

namespace PacMan.GameComponents.GameActs
{
    public class Marquee
    {
        enum State
        {
            Idle,
            ScrollingIn,
            Stationary,
            ScrollingOut
        }

        readonly MarqueeText[] _texts;
        MarqueeText _current;
        int _index;
        State _state;
        EggTimer _timer;
        Vector2 _pos;
        readonly TweeningFunction _tweeningFunction;
        Tweener? _tweener;
        readonly Tweener _colorTweener;
        Color _color;

        public Marquee(MarqueeText[] texts)
        {
            _timer = new EggTimer(1000.Seconds());
            _texts = texts;
            _index = -1;
            selectNext();

            _tweeningFunction = Tweener.CreateTweeningFunction<Elastic>(Easing.EaseInOut);

            var colorTweeningFunction = Tweener.CreateTweeningFunction<Linear>(Easing.EaseNone);

            _colorTweener = new Tweener(.33f, 1, .33f.Seconds(), colorTweeningFunction);
            _colorTweener.Ended += () =>
            {
                _colorTweener.Reverse();
                _colorTweener.Reset();
            };
        }

        void selectNext()
        {
            if (++_index >= _texts.Length)
            {
                _index = 0;
            }

            _current = _texts[_index];
            _state = State.Idle;
            _timer = new EggTimer(_current.TimeIdle);
            _pos = new Vector2(300, _current.YPosition);
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
                await idle();
            }

            if (_state == State.ScrollingIn)
            {
                await scrollingIn();
            }

            if (_state == State.Stationary)
            {
                await stationary();
            }

            if (_state == State.ScrollingOut)
            {
                await scrollingOut();
            }
        }

        ValueTask scrollingIn()
        {
            Debug.Assert(_tweener != null, nameof(_tweener) + " != null");

            _pos = new Vector2(_tweener.Position, _current.YPosition);

            if (_tweener.HasEnded)
            {
                _state = State.Stationary;
                _timer = new EggTimer(_current.TimeStationary);
            }

            return default;
        }

        ValueTask scrollingOut()
        {
            Debug.Assert(_tweener != null, nameof(_tweener) + " != null");

            _pos = new Vector2(_tweener.Position, _current.YPosition);

            if (_tweener.HasEnded)
            {
                _state = State.Idle;
                _timer = new EggTimer(_current.TimeIdle);
                selectNext();
            }

            return default;
        }

        ValueTask idle()
        {
            if (_timer.Finished)
            {
                _state = State.ScrollingIn;
                _tweener = new Tweener(300, 0, _current.TimeIn, _tweeningFunction);
                _timer = new EggTimer(_current.TimeOut);
            }

            return default;
        }

        ValueTask stationary()
        {
            if (_timer.Finished)
            {
                _state = State.ScrollingOut;
                _tweener = new Tweener(0, -300, _current.TimeOut, _tweeningFunction);
            }

            return default;
        }

        public async ValueTask Draw(CanvasWrapper session)
        {
            await session.DrawText(_current.Text, _pos.ToPoint(), _color);
        }
    }
}