namespace PacMan.GameComponents.GameActs;

public struct MarqueeText
{
    public required int YPosition { get; init; }

    public required TimeSpan TimeIdle { get; init; }

    public required TimeSpan TimeIn { get; init; }

    public required TimeSpan TimeStationary { get; init; }

    public required TimeSpan TimeOut { get; init; }

    public required string Text { get; init; }
}