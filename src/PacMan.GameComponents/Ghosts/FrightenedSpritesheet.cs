namespace PacMan.GameComponents.Ghosts
{
    public class FrightenedSpritesheet
    {
        public FrightenedSpritesheet(FramePair blue, FramePair white)
        {
            Blue = blue;
            White = white;
        }

        public FramePair Blue { get; }

        public FramePair White { get; }
    }
}