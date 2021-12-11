using FluentAssertions;
using PacMan.GameComponents.Primitives;
using Vogen;
using Xunit;

namespace SmallTests
{
    public class PointsTests
    {
        [Fact]
        public void Validation()
        {
            Action act = () => _ = Points.From(0);
            act.Should().ThrowExactly<ValueObjectValidationException>()
                .WithMessage("Points must be a positive value");
        }
    }
}
