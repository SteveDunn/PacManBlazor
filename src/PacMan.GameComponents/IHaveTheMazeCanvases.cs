using PacMan.GameComponents.Canvas;

namespace PacMan.GameComponents;

public interface IHaveTheMazeCanvases
{
    MazeCanvas GetForPlayer(int index);
}