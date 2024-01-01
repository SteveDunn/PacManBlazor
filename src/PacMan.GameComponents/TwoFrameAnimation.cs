﻿namespace PacMan.GameComponents;

public class TwoFrameAnimation
{
    private readonly LoopingTimer _timer;

    public TwoFrameAnimation(TimeSpan switchEvery)
    {
        _timer = new(switchEvery, () => Flag = !Flag);
    }

    public bool Flag { get; private set; }

    public void Run(CanvasTimingInformation timing) => _timer.Run(timing);
}