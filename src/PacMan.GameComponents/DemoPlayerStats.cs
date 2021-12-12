namespace PacMan.GameComponents;

public class DemoPlayerStats : PlayerStats
{
    public DemoPlayerStats(IMediator mediator) : base(0, mediator)
    {
        Lives = 1;
    }

    protected override ValueTask IncreaseScoreBy(Points amount)
    {
        return default;
    }
}