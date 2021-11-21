namespace PacMan.GameComponents.Ghosts;

public record LevelProps(
    IntroCutScene CutScene,
    FruitItem Fruit1,
    int FruitPoints,
    float PacManSpeedPc,
    float PacManDotsSpeedPc,
    float GhostSpeedPc,
    float GhostTunnelSpeedPc,
    int Elroy1DotsLeft,
    float Elroy1SpeedPc,
    int Elroy2DotsLeft,
    float Elroy2SpeedPc,
    float FrightPacManSpeedPc,
    float FrightPacManDotSpeedPc,
    float FrightGhostSpeedPc,
    int FrightGhostTime,
    int FrightGhostFlashes);