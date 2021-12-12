using PacMan.GameComponents;

namespace SmallTests.TestDoubles;

public class StubbedGameStorage : IGameStorage
{
    int _highScore = 10_000;

    public ValueTask<int> GetHighScore() => ValueTask.FromResult(_highScore);

    public ValueTask SetHighScore(int highScore)
    {
        _highScore = highScore;
            
        return ValueTask.CompletedTask;
    }
}