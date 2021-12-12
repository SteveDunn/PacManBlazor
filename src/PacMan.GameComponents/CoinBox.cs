namespace PacMan.GameComponents;

public class CoinBox : ICoinBox
{
    public void UseCredits(int amount)
    {
        if (Credits - amount < 0)
        {
            throw new InvalidOperationException("Not enough credits!");
        }

        Credits -= amount;
    }

    public int Credits { get; private set; }

    public void CoinInserted() => ++Credits;
}