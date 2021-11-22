namespace PacMan.GameComponents;

public interface IGameStorage
{
    ValueTask<int> GetHighScore();

    ValueTask SetHighScore(int highScore);
}