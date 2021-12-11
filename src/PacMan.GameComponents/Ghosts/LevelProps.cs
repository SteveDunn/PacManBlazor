namespace PacMan.GameComponents.Ghosts;

public record LevelProps(
    IntroCutScene CutScene,
    FruitItem Fruit1,
    Points FruitPoints,
    SpeedPercentage PacManSpeedPc,
    SpeedPercentage PacManDotsSpeedPc,
    SpeedPercentage GhostSpeedPc,
    SpeedPercentage GhostTunnelSpeedPc,
    int Elroy1DotsLeft,
    SpeedPercentage Elroy1SpeedPc,
    int Elroy2DotsLeft,
    SpeedPercentage Elroy2SpeedPc,
    SpeedPercentage FrightPacManSpeedPc,
    SpeedPercentage FrightPacManDotSpeedPc,
    SpeedPercentage FrightGhostSpeedPc,
    TimeSpan FrightGhostTime,
    int FrightGhostFlashes);