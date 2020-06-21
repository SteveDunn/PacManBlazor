using System;

namespace PacMan.GameComponents.Canvas
{
    public class CanvasTimingInformation
    {
        public TimeSpan TotalTime { get; private set; } = TimeSpan.Zero;
        
        public TimeSpan ElapsedTime { get; private set; }

        public void Update(float elapsedSinceLastCall)
        {
            ElapsedTime = TimeSpan.FromMilliseconds(elapsedSinceLastCall);

            TotalTime += ElapsedTime;
        }
    }
}