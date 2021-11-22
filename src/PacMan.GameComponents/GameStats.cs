using PacMan.GameComponents.Canvas;

namespace PacMan.GameComponents;

public class GameStats : IGameStats
{
    readonly IMediator _mediator;
    readonly IGameStorage _storage;
    int _currentPlayerIndex;

    List<PlayerStats> _playerStats;
    int _ghostsThatAreEyes;

    public GameStats(IMediator mediator, IGameStorage storage)
    {
        _mediator = mediator;
        _storage = storage;

        _playerStats = new();

#pragma warning disable 4014
        Reset(0);
#pragma warning restore 4014
    }

    public async ValueTask Reset(int players)
    {
        _ghostsThatAreEyes = 0;

        HighScore =
            Math.Max(await _storage.GetHighScore(), HighScore);

        HasPlayedIntroTune = false;

        IsDemo = false;

        // ReSharper disable once HeapView.ObjectAllocation.Evident
        _playerStats = new();

        for (int i = 0; i < players; i++)
        {
            _playerStats.Add(new(i, _mediator));
        }

        _currentPlayerIndex = -1;
    }

    public bool IsDemo { get; private set; }

    public bool HasPlayedIntroTune
    {
        get;
        set;
    }

    public bool AreAnyGhostsInEyeState => _ghostsThatAreEyes > 0;

    [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
    public PlayerStats ResetForDemo()
    {
        IsDemo = true;

        _playerStats = new();

        var playerStats = new DemoPlayerStats(_mediator);

        _playerStats.Add(playerStats);

        _currentPlayerIndex = -1;

        ChoseNextPlayer();

        return playerStats;
    }

    public void Update(CanvasTimingInformation timing)
    {
        CurrentPlayerStats?.Update(timing);
    }

    public PlayerStats GetPlayerStats(int index) => _playerStats[index];

    public ValueTask FruitEaten() => CurrentPlayerStats.FruitEaten();

    public int HighScore { get; private set; } = 10000;

    public bool HasPlayerStats(int playerNumber) => _playerStats.Count > playerNumber;

    public int AmountOfPlayers => _playerStats.Count;

    public bool AnyonePlaying => _currentPlayerIndex != -1;

    public bool IsGameOver => _playerStats.All(p => p.LivesRemaining == 0);

    public PlayerStats CurrentPlayerStats
    {
        get
        {
            if (_currentPlayerIndex < 0)
            {
                throw new InvalidOperationException("Nobody playing!");
            }

            return _playerStats[_currentPlayerIndex];
        }
    }

    public void ChoseNextPlayer()
    {
        PlayerStats[] players =
            _playerStats.Where(p => p.PlayerIndex > _currentPlayerIndex && p.LivesRemaining > 0)
                .ToArray();

        if (players.Length == 0)
        {
            players = _playerStats.Where(p => p.LivesRemaining > 0).ToArray();
        }

        if (players.Length > 0)
        {
            _currentPlayerIndex = players[0].PlayerIndex;
        }
        else
        {
            _currentPlayerIndex = -1;
        }
    }

    void updateHighScore()
    {
        HighScore = _playerStats.Count switch
        {
            1 => Math.Max(_playerStats[0].Score, HighScore),
            2 => Math.Max(_playerStats[1].Score, HighScore),
            _ => HighScore
        };
    }

    public async ValueTask PillEaten(CellIndex point)
    {
        await _playerStats[_currentPlayerIndex].PillEaten(point);
        updateHighScore();
    }

    public async ValueTask PowerPillEaten(CellIndex point)
    {
        await _playerStats[_currentPlayerIndex].PowerPillEaten(point);
        updateHighScore();
    }

    public void PacManEaten()
    {
        _playerStats[_currentPlayerIndex].PacManEaten();
        _ghostsThatAreEyes = 0;
    }

    public async ValueTask<int> GhostEaten()
    {
        ++_ghostsThatAreEyes;

        var points = await _playerStats[_currentPlayerIndex].GhostEaten();
        updateHighScore();
        return points;
    }

    public void LevelFinished()
    {
        CurrentPlayerStats.NewLevel();
    }

    public ValueTask HandleGhostBackInsideHouse()
    {
        --_ghostsThatAreEyes;

        return default;
    }
}