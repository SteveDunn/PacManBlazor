namespace PacMan.GameComponents;

public class DemoPlayerStats : PlayerStats
{
    public DemoPlayerStats(IMediator mediator) : base(0, mediator)
    {
        LivesRemaining = 1;
    }

    protected override ValueTask IncreaseScoreBy(int amount)
    {
        return default;
    }
}