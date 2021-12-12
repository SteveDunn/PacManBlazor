using Vogen;

namespace PacMan.GameComponents.Primitives;

[ValueObject(typeof(float))]
public readonly partial struct SpeedPercentage
{
    private static Validation Validate(float value) => value >= 0
        ? Validation.Ok
        : Validation.Invalid("Percentage must be greater than 0");

    //public static SpeedPercentage From(double d) => From((float) d);
}