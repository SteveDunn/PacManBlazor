// ReSharper disable HeapView.ObjectAllocation.Evident

using PacMan.GameComponents.Ghosts;

namespace PacMan.GameComponents;
// perfect game at https://www.bing.com/videos/search?q=Perfect+Pac+Man&&view=detail&mid=AE586ACE933BE975ADD9AE586ACE933BE975ADD9&FORM=VRDGAR
// cut scene 1 at 2:47
// cut scene 2 at 6:50
// cut scene 3 at 12:16 (snail thing)
// max level 21

public class LevelStats
{
    const int _startingAmountOfPills = 244;

    const int _maxLevel = 20;

    readonly char[] _currentMap;

    static readonly LevelProps[] _levelProps =
    [
        new(
            CutScene: IntroCutScene.None,
            Fruit1: FruitItem.Cherry,
            FruitPoints: Points.From(300),
            PacManSpeedPc:  SpeedPercentage.From(.80f),
            PacManDotsSpeedPc:  SpeedPercentage.From(.71f),
            GhostSpeedPc:  SpeedPercentage.From(.80f),
            GhostTunnelSpeedPc:  SpeedPercentage.From(.40f),
            Elroy1DotsLeft: 30,
            Elroy1SpeedPc:  SpeedPercentage.From(.90f),
            Elroy2DotsLeft: 15,
            Elroy2SpeedPc:  SpeedPercentage.From(.95f),
            FrightPacManSpeedPc:  SpeedPercentage.From(.90f),
            FrightPacManDotSpeedPc:  SpeedPercentage.From(.79f),
            FrightGhostSpeedPc:  SpeedPercentage.From(.50f),
            FrightGhostTime: GhostFrightDuration.SixSeconds,
            FrightGhostFlashes: 5),
        new(
            CutScene: IntroCutScene.None,
            Fruit1: FruitItem.Strawberry,
            FruitPoints: Points.From(300),
            PacManSpeedPc:  SpeedPercentage.From(.90f),
            PacManDotsSpeedPc:  SpeedPercentage.From(.79f),
            GhostSpeedPc:  SpeedPercentage.From(.85f),
            GhostTunnelSpeedPc:  SpeedPercentage.From(.45f),
            Elroy1DotsLeft: 30,
            Elroy1SpeedPc:  SpeedPercentage.From(.90f),
            Elroy2DotsLeft: 15,
            Elroy2SpeedPc:  SpeedPercentage.From(.95f),
            FrightPacManSpeedPc:  SpeedPercentage.From(.95f),
            FrightPacManDotSpeedPc:  SpeedPercentage.From(.83f),
            FrightGhostSpeedPc:  SpeedPercentage.From(.55f),
            FrightGhostTime: GhostFrightDuration.FiveSeconds,
            FrightGhostFlashes: 5),
        new(
            CutScene: IntroCutScene.BigPac,
            Fruit1: FruitItem.Peach,
            FruitPoints: Points.From(500),
            PacManSpeedPc:  SpeedPercentage.From(.90f),
            PacManDotsSpeedPc:  SpeedPercentage.From(.79f),
            GhostSpeedPc:  SpeedPercentage.From(.85f),
            GhostTunnelSpeedPc:  SpeedPercentage.From(.45f),
            Elroy1DotsLeft: 40,
            Elroy1SpeedPc:  SpeedPercentage.From(.90f),
            Elroy2DotsLeft: 20,
            Elroy2SpeedPc:  SpeedPercentage.From(.95f),
            FrightPacManSpeedPc:  SpeedPercentage.From(.95f),
            FrightPacManDotSpeedPc:  SpeedPercentage.From(.83f),
            FrightGhostSpeedPc:  SpeedPercentage.From(.55f),
            FrightGhostTime: GhostFrightDuration.FourSeconds,
            FrightGhostFlashes: 5),
        new(
            CutScene: IntroCutScene.None,
            Fruit1: FruitItem.Peach,
            FruitPoints: Points.From(500),
            PacManSpeedPc:  SpeedPercentage.From(.90f),
            PacManDotsSpeedPc:  SpeedPercentage.From(.79f),
            GhostSpeedPc:  SpeedPercentage.From(.85f),
            GhostTunnelSpeedPc:  SpeedPercentage.From(.45f),
            Elroy1DotsLeft: 40,
            Elroy1SpeedPc:  SpeedPercentage.From(.90f),
            Elroy2DotsLeft: 20,
            Elroy2SpeedPc:  SpeedPercentage.From(.95f),
            FrightPacManSpeedPc:  SpeedPercentage.From(.95f),
            FrightPacManDotSpeedPc:  SpeedPercentage.From(.83f),
            FrightGhostSpeedPc:  SpeedPercentage.From(.55f),
            FrightGhostTime: GhostFrightDuration.ThreeSeconds,
            FrightGhostFlashes: 5),
        new(
            CutScene: IntroCutScene.None,
            Fruit1: FruitItem.Apple,
            FruitPoints: Points.From(700),
            PacManSpeedPc:  SpeedPercentage.From(.100f),
            PacManDotsSpeedPc:  SpeedPercentage.From(.87f),
            GhostSpeedPc:  SpeedPercentage.From(.95f),
            GhostTunnelSpeedPc:  SpeedPercentage.From(.50f),
            Elroy1DotsLeft: 40,
            Elroy1SpeedPc:  SpeedPercentage.From(.100f),
            Elroy2DotsLeft: 20,
            Elroy2SpeedPc:  SpeedPercentage.From(.105f),
            FrightPacManSpeedPc:  SpeedPercentage.From(.100f),
            FrightPacManDotSpeedPc:  SpeedPercentage.From(.87f),
            FrightGhostSpeedPc:  SpeedPercentage.From(.60f),
            FrightGhostTime: GhostFrightDuration.TwoSeconds,
            FrightGhostFlashes: 5),
        new(
            CutScene: IntroCutScene.GhostSnagged,
            Fruit1: FruitItem.Apple,
            FruitPoints: Points.From(700),
            PacManSpeedPc:  SpeedPercentage.From(.100f),
            PacManDotsSpeedPc:  SpeedPercentage.From(.87f),
            GhostSpeedPc:  SpeedPercentage.From(.95f),
            GhostTunnelSpeedPc:  SpeedPercentage.From(.50f),
            Elroy1DotsLeft: 50,
            Elroy1SpeedPc:  SpeedPercentage.From(.100f),
            Elroy2DotsLeft: 25,
            Elroy2SpeedPc:  SpeedPercentage.From(.105f),
            FrightPacManSpeedPc:  SpeedPercentage.From(.100f),
            FrightPacManDotSpeedPc:  SpeedPercentage.From(.87f),
            FrightGhostSpeedPc:  SpeedPercentage.From(.60f),
            FrightGhostTime: GhostFrightDuration.TwoSeconds,
            FrightGhostFlashes: 5),
        new(
            CutScene: IntroCutScene.None,
            Fruit1: FruitItem.Grape,
            FruitPoints: Points.From(1000),
            PacManSpeedPc:  SpeedPercentage.From(.100f),
            PacManDotsSpeedPc:  SpeedPercentage.From(.87f),
            GhostSpeedPc:  SpeedPercentage.From(.95f),
            GhostTunnelSpeedPc:  SpeedPercentage.From(.50f),
            Elroy1DotsLeft: 50,
            Elroy1SpeedPc:  SpeedPercentage.From(.100f),
            Elroy2DotsLeft: 25,
            Elroy2SpeedPc:  SpeedPercentage.From(.105f),
            FrightPacManSpeedPc:  SpeedPercentage.From(.100f),
            FrightPacManDotSpeedPc:  SpeedPercentage.From(.87f),
            FrightGhostSpeedPc:  SpeedPercentage.From(.60f),
            FrightGhostTime: GhostFrightDuration.TwoSeconds,
            FrightGhostFlashes: 5),
        new(
            CutScene: IntroCutScene.None,
            Fruit1: FruitItem.Grape,
            FruitPoints: Points.From(1000),
            PacManSpeedPc:  SpeedPercentage.From(.100f),
            PacManDotsSpeedPc:  SpeedPercentage.From(.87f),
            GhostSpeedPc:  SpeedPercentage.From(.95f),
            GhostTunnelSpeedPc:  SpeedPercentage.From(.50f),
            Elroy1DotsLeft: 50,
            Elroy1SpeedPc:  SpeedPercentage.From(.100f),
            Elroy2DotsLeft: 25,
            Elroy2SpeedPc:  SpeedPercentage.From(.105f),
            FrightPacManSpeedPc:  SpeedPercentage.From(.100f),
            FrightPacManDotSpeedPc:  SpeedPercentage.From(.87f),
            FrightGhostSpeedPc:  SpeedPercentage.From(.60f),
            FrightGhostTime: GhostFrightDuration.OneSecond,
            FrightGhostFlashes: 5),
        new(
            CutScene: IntroCutScene.None,
            Fruit1: FruitItem.Galaxian,
            FruitPoints: Points.From(2000),
            PacManSpeedPc:  SpeedPercentage.From(.100f),
            PacManDotsSpeedPc:  SpeedPercentage.From(.87f),
            GhostSpeedPc:  SpeedPercentage.From(.95f),
            GhostTunnelSpeedPc:  SpeedPercentage.From(.50f),
            Elroy1DotsLeft: 60,
            Elroy1SpeedPc:  SpeedPercentage.From(.100f),
            Elroy2DotsLeft: 30,
            Elroy2SpeedPc:  SpeedPercentage.From(.105f),
            FrightPacManSpeedPc:  SpeedPercentage.From(.100f),
            FrightPacManDotSpeedPc:  SpeedPercentage.From(.87f),
            FrightGhostSpeedPc:  SpeedPercentage.From(.60f),
            FrightGhostTime: GhostFrightDuration.FiveSeconds,
            FrightGhostFlashes: 3),
        new(
            CutScene: IntroCutScene.TornGhostAndWorm,
            Fruit1: FruitItem.Galaxian,
            FruitPoints: Points.From(2000),
            PacManSpeedPc:  SpeedPercentage.From(.100f),
            PacManDotsSpeedPc:  SpeedPercentage.From(.87f),
            GhostSpeedPc:  SpeedPercentage.From(.95f),
            GhostTunnelSpeedPc:  SpeedPercentage.From(.50f),
            Elroy1DotsLeft: 60,
            Elroy1SpeedPc:  SpeedPercentage.From(.100f),
            Elroy2DotsLeft: 30,
            Elroy2SpeedPc:  SpeedPercentage.From(.105f),
            FrightPacManSpeedPc:  SpeedPercentage.From(.100f),
            FrightPacManDotSpeedPc:  SpeedPercentage.From(.87f),
            FrightGhostSpeedPc:  SpeedPercentage.From(.60f),
            FrightGhostTime: GhostFrightDuration.TwoSeconds,
            FrightGhostFlashes: 5),
        new(
            CutScene: IntroCutScene.None,
            Fruit1: FruitItem.Bell,
            FruitPoints: Points.From(3000),
            PacManSpeedPc:  SpeedPercentage.From(.100f),
            PacManDotsSpeedPc:  SpeedPercentage.From(.87f),
            GhostSpeedPc:  SpeedPercentage.From(.95f),
            GhostTunnelSpeedPc:  SpeedPercentage.From(.50f),
            Elroy1DotsLeft: 60,
            Elroy1SpeedPc:  SpeedPercentage.From(.100f),
            Elroy2DotsLeft: 30,
            Elroy2SpeedPc:  SpeedPercentage.From(.105f),
            FrightPacManSpeedPc:  SpeedPercentage.From(.100f),
            FrightPacManDotSpeedPc:  SpeedPercentage.From(.87f),
            FrightGhostSpeedPc:  SpeedPercentage.From(.60f),
            FrightGhostTime: GhostFrightDuration.OneSecond,
            FrightGhostFlashes: 5),
        new(
            CutScene: IntroCutScene.None,
            Fruit1: FruitItem.Bell,
            FruitPoints: Points.From(3000),
            PacManSpeedPc:  SpeedPercentage.From(.100f),
            PacManDotsSpeedPc:  SpeedPercentage.From(.87f),
            GhostSpeedPc:  SpeedPercentage.From(.95f),
            GhostTunnelSpeedPc:  SpeedPercentage.From(.50f),
            Elroy1DotsLeft: 80,
            Elroy1SpeedPc:  SpeedPercentage.From(.100f),
            Elroy2DotsLeft: 40,
            Elroy2SpeedPc:  SpeedPercentage.From(.105f),
            FrightPacManSpeedPc:  SpeedPercentage.From(.100f),
            FrightPacManDotSpeedPc:  SpeedPercentage.From(.87f),
            FrightGhostSpeedPc:  SpeedPercentage.From(.60f),
            FrightGhostTime: GhostFrightDuration.OneSecond,
            FrightGhostFlashes: 3),
        new(
            CutScene: IntroCutScene.None,
            Fruit1: FruitItem.Key,
            FruitPoints: Points.From(5000),
            PacManSpeedPc:  SpeedPercentage.From(.100f),
            PacManDotsSpeedPc:  SpeedPercentage.From(.87f),
            GhostSpeedPc:  SpeedPercentage.From(.95f),
            GhostTunnelSpeedPc:  SpeedPercentage.From(.50f),
            Elroy1DotsLeft: 80,
            Elroy1SpeedPc:  SpeedPercentage.From(.100f),
            Elroy2DotsLeft: 40,
            Elroy2SpeedPc:  SpeedPercentage.From(.105f),
            FrightPacManSpeedPc:  SpeedPercentage.From(.100f),
            FrightPacManDotSpeedPc:  SpeedPercentage.From(.87f),
            FrightGhostSpeedPc:  SpeedPercentage.From(.60f),
            FrightGhostTime:  GhostFrightDuration.OneSecond,
            FrightGhostFlashes: 3),
        new(
            CutScene: IntroCutScene.TornGhostAndWorm,
            Fruit1: FruitItem.Key,
            FruitPoints: Points.From(5000),
            PacManSpeedPc:  SpeedPercentage.From(.100f),
            PacManDotsSpeedPc:  SpeedPercentage.From(.87f),
            GhostSpeedPc:  SpeedPercentage.From(.95f),
            GhostTunnelSpeedPc:  SpeedPercentage.From(.50f),
            Elroy1DotsLeft: 80,
            Elroy1SpeedPc:  SpeedPercentage.From(.100f),
            Elroy2DotsLeft: 40,
            Elroy2SpeedPc:  SpeedPercentage.From(.105f),
            FrightPacManSpeedPc:  SpeedPercentage.From(.100f),
            FrightPacManDotSpeedPc:  SpeedPercentage.From(.87f),
            FrightGhostSpeedPc:  SpeedPercentage.From(.60f),
            FrightGhostTime:  GhostFrightDuration.ThreeSeconds,
            FrightGhostFlashes: 5),
        new(
            CutScene: IntroCutScene.None,
            Fruit1: FruitItem.Key,
            FruitPoints: Points.From(5000),
            PacManSpeedPc:  SpeedPercentage.From(.100f),
            PacManDotsSpeedPc:  SpeedPercentage.From(.87f),
            GhostSpeedPc:  SpeedPercentage.From(.95f),
            GhostTunnelSpeedPc:  SpeedPercentage.From(.50f),
            Elroy1DotsLeft: 100,
            Elroy1SpeedPc:  SpeedPercentage.From(.100f),
            Elroy2DotsLeft: 50,
            Elroy2SpeedPc:  SpeedPercentage.From(.105f),
            FrightPacManSpeedPc:  SpeedPercentage.From(.100f),
            FrightPacManDotSpeedPc:  SpeedPercentage.From(.87f),
            FrightGhostSpeedPc:  SpeedPercentage.From(.60f),
            FrightGhostTime:  GhostFrightDuration.OneSecond,
            FrightGhostFlashes: 3),
        new(
            CutScene: IntroCutScene.None,
            Fruit1: FruitItem.Key,
            FruitPoints: Points.From(5000),
            PacManSpeedPc:  SpeedPercentage.From(.100f),
            PacManDotsSpeedPc:  SpeedPercentage.From(.87f),
            GhostSpeedPc:  SpeedPercentage.From(.95f),
            GhostTunnelSpeedPc:  SpeedPercentage.From(.50f),
            Elroy1DotsLeft: 100,
            Elroy1SpeedPc:  SpeedPercentage.From(.100f),
            Elroy2DotsLeft: 50,
            Elroy2SpeedPc:  SpeedPercentage.From(.105f),
            FrightPacManSpeedPc:  SpeedPercentage.From(.100f),
            FrightPacManDotSpeedPc:  SpeedPercentage.From(.87f),
            FrightGhostSpeedPc:  SpeedPercentage.From(.60f),
            FrightGhostTime:  GhostFrightDuration.OneSecond,
            FrightGhostFlashes: 3),
        new(
            CutScene: IntroCutScene.None,
            Fruit1: FruitItem.Key,
            FruitPoints: Points.From(5000),
            PacManSpeedPc:  SpeedPercentage.From(.100f),
            PacManDotsSpeedPc:  SpeedPercentage.From(.87f),
            GhostSpeedPc:  SpeedPercentage.From(.95f),
            GhostTunnelSpeedPc:  SpeedPercentage.From(.50f),
            Elroy1DotsLeft: 100,
            Elroy1SpeedPc:  SpeedPercentage.From(.100f),
            Elroy2DotsLeft: 50,
            Elroy2SpeedPc:  SpeedPercentage.From(.105f),
            FrightPacManSpeedPc:  SpeedPercentage.From(.0f),
            FrightPacManDotSpeedPc:  SpeedPercentage.From(.0f),
            FrightGhostSpeedPc:  SpeedPercentage.From(.0f),
            FrightGhostTime:  GhostFrightDuration.ZeroSeconds,
            FrightGhostFlashes: 0),
        new(
            CutScene: IntroCutScene.TornGhostAndWorm,
            Fruit1: FruitItem.Key,
            FruitPoints: Points.From(5000),
            PacManSpeedPc:  SpeedPercentage.From(.100f),
            PacManDotsSpeedPc:  SpeedPercentage.From(.87f),
            GhostSpeedPc:  SpeedPercentage.From(.95f),
            GhostTunnelSpeedPc:  SpeedPercentage.From(.50f),
            Elroy1DotsLeft: 100,
            Elroy1SpeedPc:  SpeedPercentage.From(.100f),
            Elroy2DotsLeft: 50,
            Elroy2SpeedPc:  SpeedPercentage.From(.105f),
            FrightPacManSpeedPc:  SpeedPercentage.From(.100f),
            FrightPacManDotSpeedPc:  SpeedPercentage.From(.87f),
            FrightGhostSpeedPc:  SpeedPercentage.From(.60f),
            FrightGhostTime:  GhostFrightDuration.OneSecond,
            FrightGhostFlashes: 3),
        new(
            CutScene: IntroCutScene.None,
            Fruit1: FruitItem.Key,
            FruitPoints: Points.From(5000),
            PacManSpeedPc:  SpeedPercentage.From(.100f),
            PacManDotsSpeedPc:  SpeedPercentage.From(.87f),
            GhostSpeedPc:  SpeedPercentage.From(.95f),
            GhostTunnelSpeedPc:  SpeedPercentage.From(.50f),
            Elroy1DotsLeft: 120,
            Elroy1SpeedPc:  SpeedPercentage.From(.100f),
            Elroy2DotsLeft: 60,
            Elroy2SpeedPc:  SpeedPercentage.From(.105f),
            FrightPacManSpeedPc:  SpeedPercentage.From(.0f),
            FrightPacManDotSpeedPc:  SpeedPercentage.From(.0f),
            FrightGhostSpeedPc:  SpeedPercentage.From(.0f),
            FrightGhostTime:  GhostFrightDuration.ZeroSeconds,
            FrightGhostFlashes: 0),
        new(
            CutScene: IntroCutScene.None,
            Fruit1: FruitItem.Key,
            FruitPoints: Points.From(5000),
            PacManSpeedPc:  SpeedPercentage.From(.100f),
            PacManDotsSpeedPc:  SpeedPercentage.From(.87f),
            GhostSpeedPc:  SpeedPercentage.From(.95f),
            GhostTunnelSpeedPc:  SpeedPercentage.From(.50f),
            Elroy1DotsLeft: 120,
            Elroy1SpeedPc:  SpeedPercentage.From(.100f),
            Elroy2DotsLeft: 60,
            Elroy2SpeedPc:  SpeedPercentage.From(.105f),
            FrightPacManSpeedPc:  SpeedPercentage.From(.0f),
            FrightPacManDotSpeedPc:  SpeedPercentage.From(.0f),
            FrightGhostSpeedPc:  SpeedPercentage.From(.0f),
            FrightGhostTime:  GhostFrightDuration.ZeroSeconds,
            FrightGhostFlashes: 0),
        new(
            CutScene: IntroCutScene.None,
            Fruit1: FruitItem.Key,
            FruitPoints: Points.From(5000),
            PacManSpeedPc:  SpeedPercentage.From(.9f),
            PacManDotsSpeedPc:  SpeedPercentage.From(.79f),
            GhostSpeedPc:  SpeedPercentage.From(.95f),
            GhostTunnelSpeedPc:  SpeedPercentage.From(.50f),
            Elroy1DotsLeft: 120,
            Elroy1SpeedPc:  SpeedPercentage.From(.100f),
            Elroy2DotsLeft: 60,
            Elroy2SpeedPc:  SpeedPercentage.From(.105f),
            FrightPacManSpeedPc:  SpeedPercentage.From(.0f),
            FrightPacManDotSpeedPc:  SpeedPercentage.From(.0f),
            FrightGhostSpeedPc:  SpeedPercentage.From(.0f),
            FrightGhostTime:  GhostFrightDuration.ZeroSeconds,
            FrightGhostFlashes: 0)
    ];

    // todo: move to another class (and related properties)
    [SuppressMessage(category: "ReSharper", checkId: "StringLiteralTypo")] static readonly char[] _map = (

        // 0,0                     29,0

        "                             " +
        " oooooooooooo  oooooooooooo  " +
        " o    o     o  o     o    o  " +
        " *    o     o  o     o    *  " +
        " o    o     o  o     o    o  " +
        " oooooooooooooooooooooooooo  " +
        " o    o  o        o  o    o  " +
        " o    o  o        o  o    o  " +
        " oooooo  oooo  oooo  oooooo  " +
        "      o     +  +     o       " +
        "      o     +  +     o       " +
        "      o  ++++++++++  o       " +
        "      o  +        +  o       " +
        "      o  +        +  o       " +
        "++++++o+++        +++o+++++++" +
        "      o  +        +  o       " +
        "      o  +        +  o       " +
        "      o  ++++++++++  o       " +
        "      o  +        +  o       " +
        "      o  +        +  o       " +
        " oooooooooooo  oooooooooooo  " +
        " o    o     o  o     o    o  " +
        " o    o     o  o     o    o  " +
        " *oo  ooooooo++ooooooo  oo*  " +
        "   o  o  o        o  o  o    " +
        "   o  o  o        o  o  o    " +
        " oooooo  oooo  oooo  oooooo  " +
        " o          o  o          o  " +
        " o          o  o          o  " +
        " oooooooooooooooooooooooooo  " +
        "                             ").ToCharArray();
    // 0,30                   //27,29

    public LevelStats(int levelNumber)
    {
        LevelNumber = levelNumber;

        PillsRemaining = _startingAmountOfPills;
        _currentMap = (char[]) _map.Clone();

        FruitSession = new();
    }

    public FruitSession FruitSession { get; }

    public int PillsRemaining { get; private set; }

    public int PillsEaten => _startingAmountOfPills - PillsRemaining;

    public int LevelNumber { get; }

    public LevelProps GetLevelProps()
    {
        var index = Math.Min(val1: LevelNumber, val2: _maxLevel);
        return _levelProps[index];
    }

    public static LevelProps GetLevelProps(int level) => _levelProps[level];

    public GhostsLevelPatternProperties GetGhostPatternProperties()
    {
        // debug:
        // if (this.levelNumber === 0) {
        //     p.Scatter1 = 111117;
        //     p.Chase1 = 20;
        //     p.Scatter2 = 7;
        //     p.Chase2 = 20;
        //     p.Scatter3 = 5;
        //     p.Chase3 = 20;
        //     p.Scatter4 = 5;
        //     p.Chase4 = Number.MAX_VALUE;

        // return p;
        // }

        if (LevelNumber == 0)
        {
            return new() {
                Scatter1 = 7,
                Chase1 = 20,
                Scatter2 = 7,
                Chase2 = 20,
                Scatter3 = 5,
                Chase3 = 20,
                Scatter4 = 5,
                Chase4 = int.MaxValue
            };
        }

        if (LevelNumber >= 1 && LevelNumber <= 3)
        {
            return new() {
                Scatter1 = 7,
                Chase1 = 20,
                Scatter2 = 7,
                Chase2 = 20,
                Scatter3 = 5,
                Chase3 = 1033,
                Scatter4 = 0,
                Chase4 = int.MaxValue
            };
        }

        return new() {
            Scatter1 = 5,
            Chase1 = 20,
            Scatter2 = 7,
            Chase2 = 20,
            Scatter3 = 5,
            Chase3 = 1037,
            Scatter4 = 0,
            Chase4 = int.MaxValue
        };
    }

    public void PillEaten(in CellIndex cellPosition)
    {
        FruitSession.PillEaten();

        --PillsRemaining;

        var index = (cellPosition.Y * 29) + cellPosition.X;

        _currentMap[index] = '+';
    }

    static int GetArrayIndex(CellIndex point) => (point.Y * 29) + point.X;

    public char GetCellContent(CellIndex point) => _currentMap[GetArrayIndex(point: point)];
}