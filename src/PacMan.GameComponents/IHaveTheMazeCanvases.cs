namespace PacMan.GameComponents;

public interface IHaveTheMazeCanvases
{
    MazeCanvas GetForPlayer(int index);
}