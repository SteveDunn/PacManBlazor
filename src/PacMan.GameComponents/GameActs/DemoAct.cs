namespace PacMan.GameComponents.GameActs;

/// <summary>
/// This is almost identical to the game act, except it transitions to the 'attract screen act'.
/// </summary>
[UsedImplicitly]
public class DemoAct : IAct
{
    readonly IMediator _mediator;
    readonly IFruit _fruit;
    readonly IMaze _maze;
    readonly IPacMan _pacman;
    readonly IGhostCollection _ghostCollection;
    readonly IHumanInterfaceParser _input;
    readonly IGameStats _gameStats;

    public DemoAct(
        IMediator mediator,
        IFruit fruit,
        IMaze maze,
        IPacMan pacman,
        IGhostCollection ghostCollection,
        IHumanInterfaceParser input,
        IGameStats gameStats)
    {
        _mediator = mediator;
        _fruit = fruit;
        _maze = maze;
        _pacman = pacman;
        _ghostCollection = ghostCollection;
        _input = input;
        _gameStats = gameStats;
    }

    public string Name { get; } = "DemoAct";

    public async ValueTask Reset()
    {
        // ReSharper disable once HeapView.BoxingAllocation
        await _mediator.Send(new GetActRequest("AttractAct"));

        Pnrg.ResetPnrg();
    }

    public async ValueTask<ActUpdateResult> Update(CanvasTimingInformation timing)
    {
        if (_input.WasKeyPressedAndReleased(Keys.Five))
        {
            await _mediator.Publish(new CoinInsertedEvent());

            return ActUpdateResult.Finished;
        }

        _gameStats.Update(timing);

        await _maze.Update(timing);
        await _pacman.Update(timing);
        await _fruit.Update(timing);
        await _ghostCollection.Update(timing);

        return ActUpdateResult.Running;
    }

    public async ValueTask Draw(CanvasWrapper canvas)
    {
        await _maze.Draw(canvas);
        await _pacman.Draw(canvas);
        await _ghostCollection.DrawAll(canvas);
        await canvas.DrawMyText("GAME OVER", TextPoints.GameOverPoint, Colors.Red);
    }
}