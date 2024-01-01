namespace PacMan.GameComponents;

public class MazeCanvases : IHaveTheMazeCanvases
{
    private static readonly MazeCanvas[] _mazeCanvases = new MazeCanvas[2];
    private static bool _populated;

    public MazeCanvas GetForPlayer(int index)
    {
        if (!_populated)
        {
            throw new InvalidOperationException("Cannot get maze canvas for player as they've not been set.");
        }

        return _mazeCanvases[index];
    }

    public static void Populate(MazeCanvas player1Maze, MazeCanvas player2Maze)
    {
        _mazeCanvases[0] = player1Maze ?? throw new InvalidOperationException("no canvas!");
        _mazeCanvases[1] = player2Maze ?? throw new InvalidOperationException("no canvas!");
        _populated = true;
    }
}