using PacMan.GameComponents.Ghosts;

namespace PacMan.GameComponents;

public class PlayerStats
{
    readonly IMediator _mediator;

    Score _score = Score.Zero;
    bool _alreadyDecreasedInitialLives;
    GhostHouseDoor _ghostHouseDoor;
    LevelStats _levelStats;
    int _levelNumber;

    GhostMovementConductor _ghostMovementConductor;

    readonly List<int> _extraLives;

    [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
    public PlayerStats(int playerIndex, IMediator mediator)
    {
        _mediator = mediator;
        PlayerIndex = playerIndex;

        Score = Score.Zero;

        // cheat
        Lives = Constants.PacManLives;
        _levelNumber = -1;

        _extraLives = [10_000];
        _levelStats = new(0);
        _ghostHouseDoor = new(0, _mediator);

        var props = LevelStats.GetGhostPatternProperties();

        _ghostMovementConductor = new(props);
    }

    public ref Score Score => ref _score;

    public void Update(CanvasTimingInformation timing)
    {
        if (FrightSession != null && !FrightSession.IsFinished)
        {
            FrightSession.Update(timing);
        }
        else
        {
            _ghostMovementConductor.Update(timing);
        }

        _ghostHouseDoor.Update(timing);
    }

    public GhostMovementConductor ghostMoveConductor => _ghostMovementConductor;

    public GhostHouseDoor ghostHouseDoor => _ghostHouseDoor;

    [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
    public void NewLevel()
    {
        _levelStats = new(++_levelNumber);
        _ghostHouseDoor = new(0, _mediator);

        var props = LevelStats.GetGhostPatternProperties();

        _ghostMovementConductor = new(props);
    }

    public int PlayerIndex { get; }

    public LevelStats LevelStats => _levelStats;

    /// <summary>
    /// We initially start with 4 lives but take 1 off when
    /// Pac-Man is waiting at the very start of the game.
    /// You can see this by 3 Pac-Man lives in the bottom left,
    /// which then go to 2 when Pac-Man is displayed.
    /// </summary>
    public int Lives { get; protected set; }

    protected virtual async ValueTask IncreaseScoreBy(Points amount)
    {
        Score.IncreaseBy(amount);

        if (_extraLives.Count == 0)
        {
            return;
        }

        if (Score >= _extraLives[0])
        {
            await _mediator.Publish(new ExtraLifeEvent());

            Lives += 1;

            _extraLives.RemoveAt(0);
        }
    }

    /// <summary>
    /// Called by <see cref="GameStats"/>. Don't call it yourself
    /// as the game coordinates other things.
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public async ValueTask PillEaten(CellIndex point)
    {
        await _ghostHouseDoor.PillEaten();
        await IncreaseScoreBy(Points.From(10));
        _levelStats.PillEaten(point);
    }

    public async ValueTask FruitEaten()
    {
        await IncreaseScoreBy(_levelStats.GetLevelProps().FruitPoints);
        LevelStats.FruitSession.FruitEaten();
    }

    public GhostFrightSession? FrightSession { get; private set; }

    public bool IsInFrightSession => FrightSession != null && !FrightSession.IsFinished;

    public async ValueTask PowerPillEaten(CellIndex point)
    {
        FrightSession = new(_levelStats.GetLevelProps());

        await _ghostHouseDoor.PillEaten();
        await IncreaseScoreBy(Points.From(50));
        _levelStats.PillEaten(point);
    }

    public void PacManEaten()
    {
        _ghostHouseDoor.SwitchToUseGlobalCounter();

        var props = LevelStats.GetGhostPatternProperties();

        _ghostMovementConductor = new(props);
    }

    public void DecreaseLives()
    {
        Lives -= 1;
    }

    // If the current player has never played, they start of with 3 'remaining' lives
    // and one of them is removed, so they have the current life and 2 remaining.
    // We don't want to remove lives on every level though.
    public void TryDecreaseInitialLives()
    {
        if (_alreadyDecreasedInitialLives)
        {
            return;
        }

        _alreadyDecreasedInitialLives = true;
        Lives -= 1;
    }

    public async ValueTask<Points> GhostEaten()
    {
        if (FrightSession == null)
        {
            throw new InvalidOperationException("ghost can't be eaten as there's no fright session");
        }

        var points = FrightSession.GhostEaten();

        await IncreaseScoreBy(points);

        return points;
    }
}