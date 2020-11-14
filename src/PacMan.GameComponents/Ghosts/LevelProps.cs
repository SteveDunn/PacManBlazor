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
            CutScene = introCutScene;
            Fruit1 = fruit;
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

        public IntroCutScene CutScene { get; }

        public FruitItem Fruit1 { get; }

        public int FruitPoints { get; }

        public float PacManSpeedPc { get; }

        public float PacManDotsSpeedPc { get; }

        public float GhostSpeedPc { get; }

        public float GhostTunnelSpeedPc { get; }

        public int Elroy1DotsLeft { get; }

        public float Elroy1SpeedPc { get; }

        public int Elroy2DotsLeft { get; }

        public float Elroy2SpeedPc { get; }

        public float FrightPacManSpeedPc { get; }

        public float FrightPacManDotSpeedPc { get; }

        public float FrightGhostSpeedPc { get; }

        public int FrightGhostTime { get; }

        public int FrightGhostFlashes { get; }
    }
}