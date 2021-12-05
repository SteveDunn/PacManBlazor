using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using PacMan.GameComponents;
using PacMan.GameComponents.Primitives;
using Xunit;
using Points = PacMan.GameComponents.Primitives.Points;

namespace SmallTests
{
    public class ScoreTests
    {
        [Fact]
        public void Equality()
        {
            var s1 = new Score(10);
            var s2 = new Score(10);

            s1.Should().BeEquivalentTo(s2);
            s1.Value.Should().Be(s2.Value);

            (s1 == s2).Should().BeTrue();
        }

        [Fact]
        public void Zero() => Score.Zero.Should().Be(Score.Zero);

        [Fact]
        public void Defaulting()
        {
            Score s = default;
            s.Value.Should().Be(0);
        }

        [Fact]
        public void Increase()
        {
            var s = Score.Zero;
            s.IncreaseBy(Points.From(10));
            
            s.Value.Should().Be(10);
            
            Score.Zero.Value.Should().Be(0);
            
            s.IncreaseBy(Points.From(10));
            s.Value.Should().Be(20);
        }

        [Fact]
        public void Implicit_int()
        {
            var s = Score.Zero;
            s.IncreaseBy(Points.From(10));

            int val = s;
            val.Should().Be(10);
        }
    }
}
