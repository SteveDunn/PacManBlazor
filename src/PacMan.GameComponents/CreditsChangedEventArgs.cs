namespace PacMan.GameComponents
{
    public class CreditsChangedEventArgs
    {
        public CreditsChangedEventArgs(int creditsRemaining)
        {
            CreditsRemaining = creditsRemaining;
        }

        public int CreditsRemaining { get; }
    }
}