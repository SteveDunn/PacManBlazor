using FluentAssertions;
using PacMan.GameComponents.Canvas;
using Xunit;

namespace SmallTests
{
    public class CanvasTextFormatTests
    {
        [Fact]
        public void Test()
        {
            CanvasTextFormat ctf1 = new("Foo", 10);
            ctf1.FormattedString.Should().Be("10px Foo");
        }
    }
}
