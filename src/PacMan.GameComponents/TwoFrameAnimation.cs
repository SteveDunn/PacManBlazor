using System;
using PacMan.GameComponents.Canvas;

namespace PacMan.GameComponents
{
    public class TwoFrameAnimation
    {
        readonly LoopingTimer _timer;

        public TwoFrameAnimation(TimeSpan switchEvery)
        {
            _timer = new LoopingTimer(switchEvery, () => Flag = !Flag);
        }

        public bool Flag { get; private set; }

        public void Run(CanvasTimingInformation timing) => _timer.Run(timing);
    }
}