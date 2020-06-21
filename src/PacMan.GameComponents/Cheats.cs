namespace PacMan.GameComponents
{
    public static class Cheats
    {
        public static bool PacManNeverDies
        {
#if DEBUG
            get { return false; }
#else
            get { return false; }
#endif
        }

        public static bool ShowDiags
        {
#if DEBUG
            get { return true; }
#else
            get { return false; }
#endif
        }

        public static bool AllowDebugKeys
        {
#if DEBUG
            get { return true; }
#else
            get { return false; }
#endif
        }
    }
}