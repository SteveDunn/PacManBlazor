using FluentAssertions;
using MediatR;
using PacMan.GameComponents;
using PacMan.GameComponents.Primitives;
using SmallTests.TestDoubles;
using Xunit;

namespace SmallTests
{
    public class PlayerStatsTests
    {
        [Fact]
        public async Task InitialLives()
        {
            IMediator mediator = new StubbedMediator();
            PlayerStats sut = new(0, mediator);

            sut.Score.Should().Be(Score.Zero);

            // We initially take off 1 life when
            // Pac-Man is waiting at the very start of the game.
            // You can see this by 3 Pac-Man lives in the bottom left,
            // which then go to 2 when Pac-Man is displayed.
            sut.Lives.Should().Be(4);
            
            sut.TryDecreaseInitialLives();
            sut.Lives.Should().Be(3);
            
            // the 2nd time, nothing happens
            sut.TryDecreaseInitialLives();
            sut.Lives.Should().Be(3);
        }

        [Fact]
        public async Task Score_increases_when_a_pill_is_eaten()
        {
            IMediator mediator = new StubbedMediator();
            PlayerStats sut = new(0, mediator);

            sut.Score.Should().Be(Score.Zero);

            await sut.PillEaten(CellIndex.Zero);

            sut.Score.Should().Be(new Score(10));
        }

        [Fact]
        public async Task Extra_life_at_10_000()
        {
            IMediator mediator = new StubbedMediator();
            PlayerStats sut = new(0, mediator);
            sut.TryDecreaseInitialLives();

            sut.Score.Should().Be(Score.Zero);

            // p1 eats enough pills to get an extra life
            // that's 10,000 points, so 1,000 pills @ 10 points each.
            for (int i = 0; i < 1_000; i++)
            {
                await sut.PillEaten(CellIndex.Zero);
            }

            sut.Score.Value.Should().Be(10_000);

            sut.Lives.Should().Be(4);
        }

        [Fact]
        public async Task Only_one_extra_life_ever()
        {
            IMediator mediator = new StubbedMediator();
            PlayerStats sut = new(0, mediator);
            sut.TryDecreaseInitialLives();

            sut.Score.Should().Be(Score.Zero);

            // p1 eats enough pills to get an extra life
            // that's 10,000 points, so 1,000 pills @ 10 points each.
            for (int i = 0; i < 1_000; i++)
            {
                await sut.PillEaten(CellIndex.Zero);
            }

            sut.Lives.Should().Be(4);

            // p1 eats enough pills again to reach the maximum
            // score you can every get in the (original) game:
            for (int i = 0; i < (3_333_360 - 10_000) / 10; i++)
            {
                await sut.PillEaten(CellIndex.Zero);
            }

            sut.Score.Value.Should().Be(3_333_360);

            // should still only have 4 lives.
            sut.Lives.Should().Be(4);
        }
    }
}
