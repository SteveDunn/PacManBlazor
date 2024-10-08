using FluentAssertions;
using PacMan.GameComponents;
using PacMan.GameComponents.Ghosts;
using PacMan.GameComponents.Primitives;
using Xunit;

namespace SmallTests
{
    public class LevelStatsTests
    {
        /// <summary>
        /// Ensure that all properties are consistent. If any of these tests fail,
        /// ensure you haven't inadvertently changed anything.
        /// </summary>
        [Fact]
        public void RegressionTest()
        {
            LevelProps[] defaultLevelProps =
            {
                new(
                    CutScene: IntroCutScene.None,
                    Fruit1: FruitItem.Cherry,
                    FruitPoints: Points.From(300),
                    PacManSpeedPc: SpeedPercentage.From(.80f),
                    PacManDotsSpeedPc: SpeedPercentage.From(.71f),
                    GhostSpeedPc: SpeedPercentage.From(.80f),
                    GhostTunnelSpeedPc: SpeedPercentage.From(.40f),
                    Elroy1DotsLeft: 30,
                    Elroy1SpeedPc: SpeedPercentage.From(.90f),
                    Elroy2DotsLeft: 15,
                    Elroy2SpeedPc: SpeedPercentage.From(.95f),
                    FrightPacManSpeedPc: SpeedPercentage.From(.90f),
                    FrightPacManDotSpeedPc: SpeedPercentage.From(.79f),
                    FrightGhostSpeedPc: SpeedPercentage.From(.50f),
                    FrightGhostTime: GhostFrightDuration.SixSeconds,
                    FrightGhostFlashes: 5),
                new(
                    CutScene: IntroCutScene.None,
                    Fruit1: FruitItem.Strawberry,
                    FruitPoints: Points.From(300),
                    PacManSpeedPc: SpeedPercentage.From(.90f),
                    PacManDotsSpeedPc: SpeedPercentage.From(.79f),
                    GhostSpeedPc: SpeedPercentage.From(.85f),
                    GhostTunnelSpeedPc: SpeedPercentage.From(.45f),
                    Elroy1DotsLeft: 30,
                    Elroy1SpeedPc: SpeedPercentage.From(.90f),
                    Elroy2DotsLeft: 15,
                    Elroy2SpeedPc: SpeedPercentage.From(.95f),
                    FrightPacManSpeedPc: SpeedPercentage.From(.95f),
                    FrightPacManDotSpeedPc: SpeedPercentage.From(.83f),
                    FrightGhostSpeedPc: SpeedPercentage.From(.55f),
                    FrightGhostTime: GhostFrightDuration.FiveSeconds,
                    FrightGhostFlashes: 5),
                new(
                    CutScene: IntroCutScene.BigPac,
                    Fruit1: FruitItem.Peach,
                    FruitPoints: Points.From(500),
                    PacManSpeedPc: SpeedPercentage.From(.90f),
                    PacManDotsSpeedPc: SpeedPercentage.From(.79f),
                    GhostSpeedPc: SpeedPercentage.From(.85f),
                    GhostTunnelSpeedPc: SpeedPercentage.From(.45f),
                    Elroy1DotsLeft: 40,
                    Elroy1SpeedPc: SpeedPercentage.From(.90f),
                    Elroy2DotsLeft: 20,
                    Elroy2SpeedPc: SpeedPercentage.From(.95f),
                    FrightPacManSpeedPc: SpeedPercentage.From(.95f),
                    FrightPacManDotSpeedPc: SpeedPercentage.From(.83f),
                    FrightGhostSpeedPc: SpeedPercentage.From(.55f),
                    FrightGhostTime: GhostFrightDuration.FourSeconds,
                    FrightGhostFlashes: 5),
                new(
                    CutScene: IntroCutScene.None,
                    Fruit1: FruitItem.Peach,
                    FruitPoints: Points.From(500),
                    PacManSpeedPc: SpeedPercentage.From(.90f),
                    PacManDotsSpeedPc: SpeedPercentage.From(.79f),
                    GhostSpeedPc: SpeedPercentage.From(.85f),
                    GhostTunnelSpeedPc: SpeedPercentage.From(.45f),
                    Elroy1DotsLeft: 40,
                    Elroy1SpeedPc: SpeedPercentage.From(.90f),
                    Elroy2DotsLeft: 20,
                    Elroy2SpeedPc: SpeedPercentage.From(.95f),
                    FrightPacManSpeedPc: SpeedPercentage.From(.95f),
                    FrightPacManDotSpeedPc: SpeedPercentage.From(.83f),
                    FrightGhostSpeedPc: SpeedPercentage.From(.55f),
                    FrightGhostTime: GhostFrightDuration.ThreeSeconds,
                    FrightGhostFlashes: 5),
                new(
                    CutScene: IntroCutScene.None,
                    Fruit1: FruitItem.Apple,
                    FruitPoints: Points.From(700),
                    PacManSpeedPc: SpeedPercentage.From(1f),
                    PacManDotsSpeedPc: SpeedPercentage.From(.87f),
                    GhostSpeedPc: SpeedPercentage.From(.95f),
                    GhostTunnelSpeedPc: SpeedPercentage.From(.50f),
                    Elroy1DotsLeft: 40,
                    Elroy1SpeedPc: SpeedPercentage.From(1f),
                    Elroy2DotsLeft: 20,
                    Elroy2SpeedPc: SpeedPercentage.From(1.05f),
                    FrightPacManSpeedPc: SpeedPercentage.From(1f),
                    FrightPacManDotSpeedPc: SpeedPercentage.From(.87f),
                    FrightGhostSpeedPc: SpeedPercentage.From(.60f),
                    FrightGhostTime: GhostFrightDuration.TwoSeconds,
                    FrightGhostFlashes: 5),
                new(
                    CutScene: IntroCutScene.GhostSnagged,
                    Fruit1: FruitItem.Apple,
                    FruitPoints: Points.From(700),
                    PacManSpeedPc: SpeedPercentage.From(1f),
                    PacManDotsSpeedPc: SpeedPercentage.From(.87f),
                    GhostSpeedPc: SpeedPercentage.From(.95f),
                    GhostTunnelSpeedPc: SpeedPercentage.From(.50f),
                    Elroy1DotsLeft: 50,
                    Elroy1SpeedPc: SpeedPercentage.From(1f),
                    Elroy2DotsLeft: 25,
                    Elroy2SpeedPc: SpeedPercentage.From(1.05f),
                    FrightPacManSpeedPc: SpeedPercentage.From(1f),
                    FrightPacManDotSpeedPc: SpeedPercentage.From(.87f),
                    FrightGhostSpeedPc: SpeedPercentage.From(.60f),
                    FrightGhostTime: GhostFrightDuration.TwoSeconds,
                    FrightGhostFlashes: 5),
                new(
                    CutScene: IntroCutScene.None,
                    Fruit1: FruitItem.Grape,
                    FruitPoints: Points.From(1000),
                    PacManSpeedPc: SpeedPercentage.From(1f),
                    PacManDotsSpeedPc: SpeedPercentage.From(.87f),
                    GhostSpeedPc: SpeedPercentage.From(.95f),
                    GhostTunnelSpeedPc: SpeedPercentage.From(.50f),
                    Elroy1DotsLeft: 50,
                    Elroy1SpeedPc: SpeedPercentage.From(1f),
                    Elroy2DotsLeft: 25,
                    Elroy2SpeedPc: SpeedPercentage.From(1.05f),
                    FrightPacManSpeedPc: SpeedPercentage.From(1f),
                    FrightPacManDotSpeedPc: SpeedPercentage.From(.87f),
                    FrightGhostSpeedPc: SpeedPercentage.From(.60f),
                    FrightGhostTime: GhostFrightDuration.TwoSeconds,
                    FrightGhostFlashes: 5),
                new(
                    CutScene: IntroCutScene.None,
                    Fruit1: FruitItem.Grape,
                    FruitPoints: Points.From(1000),
                    PacManSpeedPc: SpeedPercentage.From(1f),
                    PacManDotsSpeedPc: SpeedPercentage.From(.87f),
                    GhostSpeedPc: SpeedPercentage.From(.95f),
                    GhostTunnelSpeedPc: SpeedPercentage.From(.50f),
                    Elroy1DotsLeft: 50,
                    Elroy1SpeedPc: SpeedPercentage.From(1f),
                    Elroy2DotsLeft: 25,
                    Elroy2SpeedPc: SpeedPercentage.From(1.05f),
                    FrightPacManSpeedPc: SpeedPercentage.From(1f),
                    FrightPacManDotSpeedPc: SpeedPercentage.From(.87f),
                    FrightGhostSpeedPc: SpeedPercentage.From(.60f),
                    FrightGhostTime: GhostFrightDuration.OneSecond,
                    FrightGhostFlashes: 5),
                new(
                    CutScene: IntroCutScene.None,
                    Fruit1: FruitItem.Galaxian,
                    FruitPoints: Points.From(2000),
                    PacManSpeedPc: SpeedPercentage.From(1f),
                    PacManDotsSpeedPc: SpeedPercentage.From(.87f),
                    GhostSpeedPc: SpeedPercentage.From(.95f),
                    GhostTunnelSpeedPc: SpeedPercentage.From(.50f),
                    Elroy1DotsLeft: 60,
                    Elroy1SpeedPc: SpeedPercentage.From(1f),
                    Elroy2DotsLeft: 30,
                    Elroy2SpeedPc: SpeedPercentage.From(1.05f),
                    FrightPacManSpeedPc: SpeedPercentage.From(1f),
                    FrightPacManDotSpeedPc: SpeedPercentage.From(.87f),
                    FrightGhostSpeedPc: SpeedPercentage.From(.60f),
                    FrightGhostTime: GhostFrightDuration.FiveSeconds,
                    FrightGhostFlashes: 3),
                new(
                    CutScene: IntroCutScene.TornGhostAndWorm,
                    Fruit1: FruitItem.Galaxian,
                    FruitPoints: Points.From(2000),
                    PacManSpeedPc: SpeedPercentage.From(1f),
                    PacManDotsSpeedPc: SpeedPercentage.From(.87f),
                    GhostSpeedPc: SpeedPercentage.From(.95f),
                    GhostTunnelSpeedPc: SpeedPercentage.From(.50f),
                    Elroy1DotsLeft: 60,
                    Elroy1SpeedPc: SpeedPercentage.From(1f),
                    Elroy2DotsLeft: 30,
                    Elroy2SpeedPc: SpeedPercentage.From(1.05f),
                    FrightPacManSpeedPc: SpeedPercentage.From(1f),
                    FrightPacManDotSpeedPc: SpeedPercentage.From(.87f),
                    FrightGhostSpeedPc: SpeedPercentage.From(.60f),
                    FrightGhostTime: GhostFrightDuration.TwoSeconds,
                    FrightGhostFlashes: 5),
                new(
                    CutScene: IntroCutScene.None,
                    Fruit1: FruitItem.Bell,
                    FruitPoints: Points.From(3000),
                    PacManSpeedPc: SpeedPercentage.From(1f),
                    PacManDotsSpeedPc: SpeedPercentage.From(.87f),
                    GhostSpeedPc: SpeedPercentage.From(.95f),
                    GhostTunnelSpeedPc: SpeedPercentage.From(.50f),
                    Elroy1DotsLeft: 60,
                    Elroy1SpeedPc: SpeedPercentage.From(1f),
                    Elroy2DotsLeft: 30,
                    Elroy2SpeedPc: SpeedPercentage.From(1.05f),
                    FrightPacManSpeedPc: SpeedPercentage.From(1f),
                    FrightPacManDotSpeedPc: SpeedPercentage.From(.87f),
                    FrightGhostSpeedPc: SpeedPercentage.From(.60f),
                    FrightGhostTime: GhostFrightDuration.OneSecond,
                    FrightGhostFlashes: 5),
                new(
                    CutScene: IntroCutScene.None,
                    Fruit1: FruitItem.Bell,
                    FruitPoints: Points.From(3000),
                    PacManSpeedPc: SpeedPercentage.From(1f),
                    PacManDotsSpeedPc: SpeedPercentage.From(.87f),
                    GhostSpeedPc: SpeedPercentage.From(.95f),
                    GhostTunnelSpeedPc: SpeedPercentage.From(.50f),
                    Elroy1DotsLeft: 80,
                    Elroy1SpeedPc: SpeedPercentage.From(1f),
                    Elroy2DotsLeft: 40,
                    Elroy2SpeedPc: SpeedPercentage.From(1.05f),
                    FrightPacManSpeedPc: SpeedPercentage.From(1f),
                    FrightPacManDotSpeedPc: SpeedPercentage.From(.87f),
                    FrightGhostSpeedPc: SpeedPercentage.From(.60f),
                    FrightGhostTime: GhostFrightDuration.OneSecond,
                    FrightGhostFlashes: 3),
                new(
                    CutScene: IntroCutScene.None,
                    Fruit1: FruitItem.Key,
                    FruitPoints: Points.From(5000),
                    PacManSpeedPc: SpeedPercentage.From(1f),
                    PacManDotsSpeedPc: SpeedPercentage.From(.87f),
                    GhostSpeedPc: SpeedPercentage.From(.95f),
                    GhostTunnelSpeedPc: SpeedPercentage.From(.50f),
                    Elroy1DotsLeft: 80,
                    Elroy1SpeedPc: SpeedPercentage.From(1f),
                    Elroy2DotsLeft: 40,
                    Elroy2SpeedPc: SpeedPercentage.From(1.05f),
                    FrightPacManSpeedPc: SpeedPercentage.From(1f),
                    FrightPacManDotSpeedPc: SpeedPercentage.From(.87f),
                    FrightGhostSpeedPc: SpeedPercentage.From(.60f),
                    FrightGhostTime: GhostFrightDuration.OneSecond,
                    FrightGhostFlashes: 3),
                new(
                    CutScene: IntroCutScene.TornGhostAndWorm,
                    Fruit1: FruitItem.Key,
                    FruitPoints: Points.From(5000),
                    PacManSpeedPc: SpeedPercentage.From(1f),
                    PacManDotsSpeedPc: SpeedPercentage.From(.87f),
                    GhostSpeedPc: SpeedPercentage.From(.95f),
                    GhostTunnelSpeedPc: SpeedPercentage.From(.50f),
                    Elroy1DotsLeft: 80,
                    Elroy1SpeedPc: SpeedPercentage.From(1f),
                    Elroy2DotsLeft: 40,
                    Elroy2SpeedPc: SpeedPercentage.From(1.05f),
                    FrightPacManSpeedPc: SpeedPercentage.From(1f),
                    FrightPacManDotSpeedPc: SpeedPercentage.From(.87f),
                    FrightGhostSpeedPc: SpeedPercentage.From(.60f),
                    FrightGhostTime: GhostFrightDuration.ThreeSeconds,
                    FrightGhostFlashes: 5),
                new(
                    CutScene: IntroCutScene.None,
                    Fruit1: FruitItem.Key,
                    FruitPoints: Points.From(5000),
                    PacManSpeedPc: SpeedPercentage.From(1f),
                    PacManDotsSpeedPc: SpeedPercentage.From(.87f),
                    GhostSpeedPc: SpeedPercentage.From(.95f),
                    GhostTunnelSpeedPc: SpeedPercentage.From(.50f),
                    Elroy1DotsLeft: 100,
                    Elroy1SpeedPc: SpeedPercentage.From(1f),
                    Elroy2DotsLeft: 50,
                    Elroy2SpeedPc: SpeedPercentage.From(1.05f),
                    FrightPacManSpeedPc: SpeedPercentage.From(1f),
                    FrightPacManDotSpeedPc: SpeedPercentage.From(.87f),
                    FrightGhostSpeedPc: SpeedPercentage.From(.60f),
                    FrightGhostTime: GhostFrightDuration.OneSecond,
                    FrightGhostFlashes: 3),
                new(
                    CutScene: IntroCutScene.None,
                    Fruit1: FruitItem.Key,
                    FruitPoints: Points.From(5000),
                    PacManSpeedPc: SpeedPercentage.From(1f),
                    PacManDotsSpeedPc: SpeedPercentage.From(.87f),
                    GhostSpeedPc: SpeedPercentage.From(.95f),
                    GhostTunnelSpeedPc: SpeedPercentage.From(.50f),
                    Elroy1DotsLeft: 100,
                    Elroy1SpeedPc: SpeedPercentage.From(1f),
                    Elroy2DotsLeft: 50,
                    Elroy2SpeedPc: SpeedPercentage.From(1.05f),
                    FrightPacManSpeedPc: SpeedPercentage.From(1f),
                    FrightPacManDotSpeedPc: SpeedPercentage.From(.87f),
                    FrightGhostSpeedPc: SpeedPercentage.From(.60f),
                    FrightGhostTime: GhostFrightDuration.OneSecond,
                    FrightGhostFlashes: 3),
                new(
                    CutScene: IntroCutScene.None,
                    Fruit1: FruitItem.Key,
                    FruitPoints: Points.From(5000),
                    PacManSpeedPc: SpeedPercentage.From(1f),
                    PacManDotsSpeedPc: SpeedPercentage.From(.87f),
                    GhostSpeedPc: SpeedPercentage.From(.95f),
                    GhostTunnelSpeedPc: SpeedPercentage.From(.50f),
                    Elroy1DotsLeft: 100,
                    Elroy1SpeedPc: SpeedPercentage.From(1f),
                    Elroy2DotsLeft: 50,
                    Elroy2SpeedPc: SpeedPercentage.From(1.05f),
                    FrightPacManSpeedPc: SpeedPercentage.From(.0f),
                    FrightPacManDotSpeedPc: SpeedPercentage.From(.0f),
                    FrightGhostSpeedPc: SpeedPercentage.From(.0f),
                    FrightGhostTime: GhostFrightDuration.ZeroSeconds,
                    FrightGhostFlashes: 0),
                new(
                    CutScene: IntroCutScene.TornGhostAndWorm,
                    Fruit1: FruitItem.Key,
                    FruitPoints: Points.From(5000),
                    PacManSpeedPc: SpeedPercentage.From(1f),
                    PacManDotsSpeedPc: SpeedPercentage.From(.87f),
                    GhostSpeedPc: SpeedPercentage.From(.95f),
                    GhostTunnelSpeedPc: SpeedPercentage.From(.50f),
                    Elroy1DotsLeft: 100,
                    Elroy1SpeedPc: SpeedPercentage.From(1f),
                    Elroy2DotsLeft: 50,
                    Elroy2SpeedPc: SpeedPercentage.From(1.05f),
                    FrightPacManSpeedPc: SpeedPercentage.From(1f),
                    FrightPacManDotSpeedPc: SpeedPercentage.From(.87f),
                    FrightGhostSpeedPc: SpeedPercentage.From(.60f),
                    FrightGhostTime: GhostFrightDuration.OneSecond,
                    FrightGhostFlashes: 3),
                new(
                    CutScene: IntroCutScene.None,
                    Fruit1: FruitItem.Key,
                    FruitPoints: Points.From(5000),
                    PacManSpeedPc: SpeedPercentage.From(1f),
                    PacManDotsSpeedPc: SpeedPercentage.From(.87f),
                    GhostSpeedPc: SpeedPercentage.From(.95f),
                    GhostTunnelSpeedPc: SpeedPercentage.From(.50f),
                    Elroy1DotsLeft: 120,
                    Elroy1SpeedPc: SpeedPercentage.From(1f),
                    Elroy2DotsLeft: 60,
                    Elroy2SpeedPc: SpeedPercentage.From(1.05f),
                    FrightPacManSpeedPc: SpeedPercentage.From(.0f),
                    FrightPacManDotSpeedPc: SpeedPercentage.From(.0f),
                    FrightGhostSpeedPc: SpeedPercentage.From(.0f),
                    FrightGhostTime: GhostFrightDuration.ZeroSeconds,
                    FrightGhostFlashes: 0),
                new(
                    CutScene: IntroCutScene.None,
                    Fruit1: FruitItem.Key,
                    FruitPoints: Points.From(5000),
                    PacManSpeedPc: SpeedPercentage.From(1f),
                    PacManDotsSpeedPc: SpeedPercentage.From(.87f),
                    GhostSpeedPc: SpeedPercentage.From(.95f),
                    GhostTunnelSpeedPc: SpeedPercentage.From(.50f),
                    Elroy1DotsLeft: 120,
                    Elroy1SpeedPc: SpeedPercentage.From(1f),
                    Elroy2DotsLeft: 60,
                    Elroy2SpeedPc: SpeedPercentage.From(1.05f),
                    FrightPacManSpeedPc: SpeedPercentage.From(.0f),
                    FrightPacManDotSpeedPc: SpeedPercentage.From(.0f),
                    FrightGhostSpeedPc: SpeedPercentage.From(.0f),
                    FrightGhostTime: GhostFrightDuration.ZeroSeconds,
                    FrightGhostFlashes: 0),

                new(
                    CutScene: IntroCutScene.None,
                    Fruit1: FruitItem.Key,
                    FruitPoints: Points.From(5000),
                    PacManSpeedPc: SpeedPercentage.From(.90f),
                    PacManDotsSpeedPc: SpeedPercentage.From(.79f),
                    GhostSpeedPc: SpeedPercentage.From(.95f),
                    GhostTunnelSpeedPc: SpeedPercentage.From(.50f),
                    Elroy1DotsLeft: 120,
                    Elroy1SpeedPc: SpeedPercentage.From(1f),
                    Elroy2DotsLeft: 60,
                    Elroy2SpeedPc: SpeedPercentage.From(1.05f),
                    FrightPacManSpeedPc: SpeedPercentage.From(.0f),
                    FrightPacManDotSpeedPc: SpeedPercentage.From(.0f),
                    FrightGhostSpeedPc: SpeedPercentage.From(.0f),
                    FrightGhostTime: GhostFrightDuration.ZeroSeconds,
                    FrightGhostFlashes: 0)
            };

            for (int i = 0; i < defaultLevelProps.Length; i++)
            {
                LevelProps sut = LevelStats.GetLevelProps(i);
                sut.Should().Be(defaultLevelProps[i]);
//                (sut == defaultLevelProps[i]).Should().BeTrue();
            }

            // make sure there's no more
            Action act = () => LevelStats.GetLevelProps(defaultLevelProps.Length);
            act.Should().ThrowExactly<IndexOutOfRangeException>();
        }
    }
}
