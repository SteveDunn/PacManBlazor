using FluentAssertions;
using PacMan.GameComponents;
using PacMan.GameComponents.Ghosts;
using Xunit;

namespace SmallTests;

public class GlobalDotCounterTests
{
    [Fact]
    public void Default()
    {
        var sut = new GlobalDotCounter(0);
        sut.CanGhostLeave(GhostNickname.Blinky).Should().BeFalse();
        sut.CanGhostLeave(GhostNickname.Pinky).Should().BeFalse();
        sut.CanGhostLeave(GhostNickname.Clyde).Should().BeFalse();
        sut.CanGhostLeave(GhostNickname.Inky).Should().BeFalse();
    }
}