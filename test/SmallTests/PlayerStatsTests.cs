using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using PacMan.GameComponents;
using SmallTests.TestDoubles;
using Xunit;

namespace SmallTests
{
    public class PlayerStatsTests
    {
        [Fact]
        public async Task Equality()
        {
            IMediator mediator = new StubbedMediator();
            PlayerStats sut = new(0, mediator);

            sut.Score.Should().Be(Score.Zero);

            await sut.PillEaten(CellIndex.Zero);

            sut.Score.Should().Be(new Score(10));
        }
    }
}
