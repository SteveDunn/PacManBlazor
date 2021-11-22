using PacMan.GameComponents.Canvas;

namespace PacMan.GameComponents;

public interface IGameStats
{
    ValueTask Reset(int players);

    bool IsDemo { get; }

    bool HasPlayedIntroTune { get; set; }

    int HighScore { get; }

    int AmountOfPlayers { get; }

    bool AnyonePlaying { get; }

    bool IsGameOver { get; }

    PlayerStats CurrentPlayerStats { get; }

    bool AreAnyGhostsInEyeState { get; }

    void Update(CanvasTimingInformation timing);

    PlayerStats GetPlayerStats(int index);

    ValueTask FruitEaten();

    bool HasPlayerStats(int playerNumber);

    void ChoseNextPlayer();

    ValueTask PillEaten(CellIndex point);

    ValueTask PowerPillEaten(CellIndex point);

    void PacManEaten();

    ValueTask<int> GhostEaten();

    void LevelFinished();

    PlayerStats ResetForDemo();

    ValueTask HandleGhostBackInsideHouse();
}