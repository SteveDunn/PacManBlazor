namespace PacMan.GameComponents
{
    public interface ICoinBox
    {
        void UseCredits(int amount);

        int Credits { get; }

        void CoinInserted();
    }
}