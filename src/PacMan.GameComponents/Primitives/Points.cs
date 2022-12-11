using Vogen;

namespace PacMan.GameComponents.Primitives;

[ValueObject]
public readonly partial struct Points
{
    private static Validation Validate(int value) =>
        value > 0 ? Validation.Ok : Validation.Invalid("Points must be a positive value");
}