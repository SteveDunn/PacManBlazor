using System;
using PacMan.GameComponents.Canvas;

namespace PacMan.GameComponents
{
    public class LoopingTimer
    {
        TimeSpan _currentTime;

        readonly TimeSpan _firesEvery;
        readonly Action _callback;

        public static LoopingTimer DoNothing => new(
            TimeSpan.MaxValue,
            static() => {
            });

        public LoopingTimer(TimeSpan firesEvery, Action callback)
        {
            _firesEvery = firesEvery;
            _currentTime = firesEvery;
            _callback = callback;
        }

        public void Run(CanvasTimingInformation timing)
        {
            _currentTime -= timing.ElapsedTime;

            if (_currentTime < TimeSpan.Zero)
            {
                _currentTime += _firesEvery;

                _callback();
            }
        }
    }
}