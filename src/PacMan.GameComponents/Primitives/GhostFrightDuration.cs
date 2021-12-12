using Vogen;

namespace PacMan.GameComponents.Primitives;

[ValueObject(typeof(TimeSpan))]
public partial struct GhostFrightDuration
{
    private static Validation Validate(TimeSpan timeSpan) =>
        timeSpan >= TimeSpan.Zero ? Validation.Ok : Validation.Invalid("Cannot be negative");

    public GhostFrightDuration DecreaseBy(TimeSpan amount)
    {
        return GhostFrightDuration.From(Value - amount);
    }

    public static readonly GhostFrightDuration SixSeconds = From(6.Seconds());
    public static readonly GhostFrightDuration FiveSeconds = From(5.Seconds());
    public static readonly GhostFrightDuration FourSeconds = From(4.Seconds());
    public static readonly GhostFrightDuration ThreeSeconds = From(3.Seconds());
    public static readonly GhostFrightDuration TwoSeconds = From(2.Seconds());
    public static readonly GhostFrightDuration OneSecond = From(1.Seconds());
    public static readonly GhostFrightDuration ZeroSeconds = From(0.Seconds());
}