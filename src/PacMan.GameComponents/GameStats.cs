﻿namespace PacMan.GameComponents;

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
        CurrentPlayerStats.Update(timing);
    }

    public PlayerStats GetPlayerStats(int index) => _playerStats[index];

    public ValueTask FruitEaten() => CurrentPlayerStats.FruitEaten();

    public int HighScore { get; private set; } = 10_000;

    public bool HasPlayerStats(int playerNumber) => _playerStats.Count > playerNumber;

    public int AmountOfPlayers => _playerStats.Count;

    public bool AnyonePlaying => _currentPlayerIndex != -1;

    public bool IsGameOver => _playerStats.All(p => p.Lives == 0);

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
        // see if any players with a higher index have lives.
        PlayerStats[] otherPlayersThatHaveLives =
            _playerStats.Where(p => p.PlayerIndex > _currentPlayerIndex && p.Lives > 0)
                .ToArray();

        // if no other players have lives, then see if any players with a lower index have lives.
        if (otherPlayersThatHaveLives.Length == 0)
        {
            otherPlayersThatHaveLives = _playerStats.Where(p => p.Lives > 0).ToArray();
        }

        // use the first one found, if any
        if (otherPlayersThatHaveLives.Length > 0)
        {
            _currentPlayerIndex = otherPlayersThatHaveLives[0].PlayerIndex;
        }
        // otherwise signify that there is no 'next player'
        else
        {
            _currentPlayerIndex = -1;
        }
    }

    void UpdateHighScore()
    {
        for (int i = 0; i < _playerStats.Count; i++)
        {
            HighScore = Math.Max(HighScore, _playerStats[i].Score);
        }
    }

    public async ValueTask PillEaten(CellIndex point)
    {
        await _playerStats[_currentPlayerIndex].PillEaten(point);
        UpdateHighScore();
    }

    public async ValueTask PowerPillEaten(CellIndex point)
    {
        await _playerStats[_currentPlayerIndex].PowerPillEaten(point);
        UpdateHighScore();
    }

    public void PacManEaten()
    {
        _playerStats[_currentPlayerIndex].PacManEaten();
        _ghostsThatAreEyes = 0;
    }

    public async ValueTask<Points> GhostEaten()
    {
        ++_ghostsThatAreEyes;

        var points = await _playerStats[_currentPlayerIndex].GhostEaten();
        UpdateHighScore();
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