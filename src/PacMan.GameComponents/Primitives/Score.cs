using Vogen;

namespace PacMan.GameComponents.Primitives;

[ValueObject<int>(toPrimitiveCasting: CastOperator.Implicit)]
public partial struct Score
{
    [Pure]
    public Score IncreaseBy(Points points) => From(Value + points.Value);

    public static readonly Score Zero = new(0);
}