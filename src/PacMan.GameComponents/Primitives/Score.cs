namespace PacMan.GameComponents.Primitives;

public record struct Score(int Value)
{
    public void IncreaseBy(Points points) => Value += points.Value;

    public static readonly Score Zero = new(0);

    public static implicit operator int(Score score) => score.Value;
}