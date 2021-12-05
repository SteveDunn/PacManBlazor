using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using PacMan.GameComponents;
using Xunit;
// ReSharper disable All

namespace SmallTests
{
    public class CellIndexTests
    {
        [Fact]
        public void Equality()
        {
            CellIndex c10_10 = new(10, 10);

            c10_10.Should().Be(new CellIndex(10,10));
            c10_10.Should().NotBe(new CellIndex(11,11));
            c10_10.Should().NotBe("??");
            c10_10.Should().NotBe(null);
            c10_10.Equals(null).Should().BeFalse();
            c10_10.Equals("!!").Should().BeFalse();
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
