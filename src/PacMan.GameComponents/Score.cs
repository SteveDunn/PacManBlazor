namespace PacMan.GameComponents;

public record struct Score(int Value)
{
    public void IncreaseBy(uint points) => Value += (int)points;

    public readonly static Score Zero = new(0);

    public static implicit operator int(Score score) => score.Value;
}