namespace PacMan.GameComponents.Ghosts
{
    public class LevelProps
    {
        public LevelProps(
            IntroCutScene introCutScene,

            FruitItem fruit,
            int fruitPoints,

            float pacManSpeedPc,
            float pacManDotsSpeedPc,

            float ghostSpeedPc,
            float ghostTunnelSpeedPc,

            int elroy1DotsLeft,
            float elroy1SpeedPc,
            int elroy2DotsLeft,
            float elroy2SpeedPc,

            float frightPacManSpeedPc,
            float frightPacManDotSpeedPc,

            float frightGhostSpeedPc,
            int frightGhostTime,
            int frightGhostFlashes)
        {
            IntroCutScene = introCutScene;
            Fruit = fruit;
            FruitPoints = fruitPoints;
            PacManSpeedPc = pacManSpeedPc;
            PacManDotsSpeedPc = pacManDotsSpeedPc;
            GhostSpeedPc = ghostSpeedPc;
            GhostTunnelSpeedPc = ghostTunnelSpeedPc;
            Elroy1DotsLeft = elroy1DotsLeft;
            Elroy1SpeedPc = elroy1SpeedPc;
            Elroy2DotsLeft = elroy2DotsLeft;
            Elroy2SpeedPc = elroy2SpeedPc;
            FrightPacManSpeedPc = frightPacManSpeedPc;
            FrightPacManDotSpeedPc = frightPacManDotSpeedPc;
            FrightGhostSpeedPc = frightGhostSpeedPc;
            FrightGhostTime = frightGhostTime;
            FrightGhostFlashes = frightGhostFlashes;
        }

        public readonly IntroCutScene IntroCutScene;

        public readonly FruitItem Fruit ;
        public readonly int FruitPoints;

        public readonly float PacManSpeedPc;
        public readonly float PacManDotsSpeedPc;

        public readonly float GhostSpeedPc;
        public readonly float GhostTunnelSpeedPc;

        public readonly int Elroy1DotsLeft;
        public readonly float Elroy1SpeedPc;
        public readonly int Elroy2DotsLeft;
        public readonly float Elroy2SpeedPc;

        public readonly float FrightPacManSpeedPc;
        public readonly float FrightPacManDotSpeedPc;

        public readonly float FrightGhostSpeedPc;
        public readonly int FrightGhostTime;
        public readonly int FrightGhostFlashes;
    }
}