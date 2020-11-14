using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using PacMan.GameComponents;
using Xunit;
// ReSharper disable All

namespace SmallTests
{
    public class LevelStatsTests
    {
        [Fact]
        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public void Equality()
        {
            LevelStats sut = new(2);
            var props = sut.GetGhostPatternProperties();
            props.Scatter1.Should().Be(7);
            props.Chase1.Should().Be(20);
            props.Scatter2.Should().Be(7);
            props.Chase2.Should().Be(20);
            props.Scatter3.Should().Be(5);
            props.Chase3.Should().Be(1033);
            props.Scatter4.Should().Be(0);
            props.Chase4.Should().Be(int.MaxValue);
        }

        [Fact]
        public void Adding()
        {
            CellIndex c10_10 = new(10, 10);

            (c10_10 + c10_10).Should().Be(new CellIndex(20,20));
        }

        [Fact]
        public void Subtracting()
        {
            CellIndex c10_10 = new(10, 10);

            (c10_10 - c10_10).Should().Be(new CellIndex(0,0));
            (c10_10 - c10_10).Should().Be(CellIndex.Zero);
        }
    }
}
