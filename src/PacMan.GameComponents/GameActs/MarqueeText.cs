using System;

namespace PacMan.GameComponents.GameActs
{
    public struct MarqueeText
    {
        public int YPosition { get; set; }
        public TimeSpan TimeIdle { get; set; }
        public TimeSpan TimeIn { get; set; }
        public TimeSpan TimeStationary { get; set; }
        public TimeSpan TimeOut { get; set; }
        public string Text { get; set; }
    }
}