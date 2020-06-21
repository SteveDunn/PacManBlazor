using System;
using PacMan.GameComponents.Canvas;

namespace PacMan.GameComponents
{
    public class EggTimer
    {
        readonly Action _whenFinished;
        readonly TimeSpan _duration;

        bool _isPaused;
        bool _isFinished;
        TimeSpan _currentTime;

        static void doNothing() { }

        public EggTimer(TimeSpan duration, Action whenFinished): this(duration) => 
            _whenFinished = whenFinished;

        public EggTimer(TimeSpan duration)
        {
            _duration = duration;
            _currentTime = duration;
            _whenFinished = doNothing;
        }

        public void Reset() => _currentTime = _duration;

        double getPercentProgress()
        {
            TimeSpan msGone = _duration - _currentTime;

            double pc = msGone.TotalMilliseconds * 100f / _duration.TotalMilliseconds;

            return pc;
        }

        public float Progress => (float) (getPercentProgress() / 100f);

        public bool Finished => _isFinished;
        public static EggTimer Unset => new EggTimer(TimeSpan.MaxValue);

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
}