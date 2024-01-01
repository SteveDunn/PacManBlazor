namespace PacMan.GameComponents.GameActs;

public class GameOverAct : IAct
{
    private readonly IMaze _maze;
    private bool _finished;

    private LoopingTimer _currentTimer = LoopingTimer.DoNothing;

    public GameOverAct(IMaze maze) => _maze = maze;

    public string Name => "GameOverAct";

    public ValueTask Reset()
    {
        _finished = false;

        _currentTimer = new(2.Seconds(), () =>
        {
            _currentTimer = new(2.Seconds(), () =>
            {
                _finished = true;
            });
        });

        return default;
    }

    public ValueTask<ActUpdateResult> Update(CanvasTimingInformation timing)
    {
        _currentTimer.Run(timing);

        return new(_finished ? ActUpdateResult.Finished : ActUpdateResult.Running);
    }

    public async ValueTask Draw(CanvasWrapper session)
    {
        await _maze.Draw(session);

        await session.DrawMyText("GAME OVER", TextPoints.GameOverPoint, Colors.Red);
    }
}