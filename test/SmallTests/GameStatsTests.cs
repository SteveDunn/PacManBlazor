using FluentAssertions;
using MediatR;
using PacMan.GameComponents;
using PacMan.GameComponents.Primitives;
using SmallTests.TestDoubles;
using Xunit;

namespace SmallTests
{
    public class GameStatsTests
    {
        [Fact]
        public async Task High_score_defaults()
        {
            IMediator mediator = new StubbedMediator();
            GameStats sut = new(mediator, new StubbedGameStorage());

            sut.HighScore.Should().Be(10_000);

            await sut.Reset(1);
            
            sut.HighScore.Should().Be(10_000);

            sut.ResetForDemo();
            
            sut.HighScore.Should().Be(10_000);
        }


        [Fact]
        public void Defaulting()
        {
            IMediator mediator = new StubbedMediator();
            GameStats sut = new(mediator, new StubbedGameStorage());

            sut.AmountOfPlayers.Should().Be(0);
            sut.AnyonePlaying.Should().BeFalse();
            sut.IsDemo.Should().BeFalse();
            sut.IsGameOver.Should().BeTrue();
            sut.HighScore.Should().Be(10_000);
        }

        [Fact]
        public async Task Resetting()
        {
            IMediator mediator = new StubbedMediator();
            GameStats sut = new(mediator, new StubbedGameStorage());
            await sut.Reset(2);

            sut.AmountOfPlayers.Should().Be(2);
            sut.AnyonePlaying.Should().BeFalse();
            sut.IsDemo.Should().BeFalse();
            sut.IsGameOver.Should().BeFalse();

            sut.GetPlayerStats(0).Score.Should().Be(Score.Zero);
            sut.GetPlayerStats(1).Score.Should().Be(Score.Zero);

            // should throw as nobody is playing yet
            Action action = () => sut.CurrentPlayerStats.PlayerIndex.Should().Be(0);
            action.Should().ThrowExactly<InvalidOperationException>().WithMessage("nobody playing!");
        }

        [Fact]
        public async Task Game_flow_2_players()
        {
            IMediator mediator = new StubbedMediator();
            GameStats sut = new(mediator, new StubbedGameStorage());
            await sut.Reset(2);

            sut.AmountOfPlayers.Should().Be(2);

            sut.ChoseNextPlayer();
            sut.AnyonePlaying.Should().BeTrue();
            
            var p1 = sut.CurrentPlayerStats;
            p1.PlayerIndex.Should().Be(0);

            // p1 starting
            p1.TryDecreaseInitialLives();
            
            // p1 eaten
            p1.DecreaseLives();

            // p2 starting
            sut.ChoseNextPlayer();
            var p2 = sut.CurrentPlayerStats;
            p2.TryDecreaseInitialLives();

            // p2 eaten
            p2.DecreaseLives();

            // p1 eaten
            sut.ChoseNextPlayer();
            p1.DecreaseLives();

            // p2 eaten
            sut.ChoseNextPlayer();
            p2.DecreaseLives();

            // p1 eaten
            sut.ChoseNextPlayer();
            p1.DecreaseLives();

            sut.ChoseNextPlayer();
            
            sut.CurrentPlayerStats.Should().Be(p2);

            // p2 eaten
            p2.DecreaseLives();

            // both players have zero lives - so switching to next one isn't possible
            sut.ChoseNextPlayer();
            sut.AnyonePlaying.Should().BeFalse();
        }

        [Fact]
        public async Task High_score_updates_as_player_1_gets_points()
        {
            IMediator mediator = new StubbedMediator();
            GameStats sut = new(mediator, new StubbedGameStorage());
            await sut.Reset(1);

            sut.ChoseNextPlayer();
            
            var p1 = sut.CurrentPlayerStats;
            p1.PlayerIndex.Should().Be(0);

            // p1 starting
            p1.TryDecreaseInitialLives();

            // p1 eats enough pills to get an extra life
            // that's 10,000 points, so 1,000 pills @ 10 points each.
            for (int i = 0; i < 1_000; i++)
            {
                await sut.PillEaten(CellIndex.Zero);
            }

            p1.Score.Value.Should().Be(10_000);
            sut.HighScore.Should().Be(10_000);

            await sut.PillEaten(CellIndex.Zero);

            p1.Score.Value.Should().Be(10_010);
            sut.HighScore.Should().Be(10_010);
        }

        [Fact]
        public async Task High_score_updates_as_player_1_in_two_player_gamegets_points()
        {
            IMediator mediator = new StubbedMediator();
            GameStats sut = new(mediator, new StubbedGameStorage());
            await sut.Reset(2);

            sut.ChoseNextPlayer();
            
            var p1 = sut.CurrentPlayerStats;
            p1.PlayerIndex.Should().Be(0);

            // p1 starting
            p1.TryDecreaseInitialLives();

            // p1 eats enough pills to get an extra life
            // that's 10,000 points, so 1,000 pills @ 10 points each.
            for (int i = 0; i < 1_000; i++)
            {
                await sut.PillEaten(CellIndex.Zero);
            }

            p1.Score.Value.Should().Be(10_000);
            sut.HighScore.Should().Be(10_000);

            await sut.PillEaten(CellIndex.Zero);

            p1.Score.Value.Should().Be(10_010);
            sut.HighScore.Should().Be(10_010);
        }

        [Fact]
        public async Task Game_flow_2_players_p1_gets_an_extra_life()
        {
            IMediator mediator = new StubbedMediator();
            GameStats sut = new(mediator, new StubbedGameStorage());
            await sut.Reset(2);

            sut.AmountOfPlayers.Should().Be(2);

            sut.ChoseNextPlayer();
            sut.AnyonePlaying.Should().BeTrue();
            
            var p1 = sut.CurrentPlayerStats;
            p1.PlayerIndex.Should().Be(0);

            // p1 starting - we initially take off 1 life when
            // Pac-Man is waiting at the very start of the game.
            // You can see this by 3 Pac-Man lives in the bottom left,
            // which then go to 2 when Pac-Man is displayed.
            p1.TryDecreaseInitialLives();

            // p1 eaten
            p1.DecreaseLives();
            sut.ChoseNextPlayer();
            
            var p2 = sut.CurrentPlayerStats;

            // p2 starting
            p2.TryDecreaseInitialLives();

            // p2 eaten
            p2.DecreaseLives();
            sut.ChoseNextPlayer();

            // p1 eaten
            p1.DecreaseLives();
            p1.Lives.Should().Be(1);
            sut.ChoseNextPlayer();
            
            // p2 eaten
            p2.DecreaseLives();
            sut.ChoseNextPlayer();
            
            // p1 eats enough pills to get an extra life
            // that's 10,000 points, so 1,000 pills @ 10 points each.
            for (int i = 0; i < 1_000; i++)
            {
                await sut.PillEaten(CellIndex.Zero);
            }

            p1.Score.Value.Should().Be(10_000);

            p1.Lives.Should().Be(2);
            
            // p1 eaten
            p1.DecreaseLives();
            sut.ChoseNextPlayer();
            
            sut.CurrentPlayerStats.Should().Be(p2);

            // p2 eaten
            p2.DecreaseLives();

            // p1 has gotten an extra life, so they should be next
            sut.ChoseNextPlayer();
            sut.AnyonePlaying.Should().BeTrue();
            sut.CurrentPlayerStats.Should().Be(p1);
            
            p1.DecreaseLives();

            // both players have zero lives - so switching to next one isn't possible
            sut.ChoseNextPlayer();
            sut.AnyonePlaying.Should().BeFalse();
        }

        [Fact]
        public void IsAnyonePlaying()
        {
            IMediator mediator = new StubbedMediator();
            GameStats sut = new(mediator, new StubbedGameStorage());
            
            Action action = () => sut.CurrentPlayerStats.PlayerIndex.Should().Be(0);

            action.Should().ThrowExactly<InvalidOperationException>().WithMessage("nobody playing!");
        }
        

        [Fact]
        public void ResetForDemo()
        {
            IMediator mediator = new StubbedMediator();
            GameStats sut = new(mediator, new StubbedGameStorage());
            sut.ResetForDemo();

            sut.AmountOfPlayers.Should().Be(1);
            sut.AnyonePlaying.Should().BeTrue();
            sut.IsDemo.Should().BeTrue();
            
            sut.HasPlayerStats(0).Should().BeTrue();
            sut.HasPlayerStats(1).Should().BeFalse();
            sut.HasPlayerStats(100).Should().BeFalse();
            
            sut.ChoseNextPlayer();
            sut.CurrentPlayerStats.PlayerIndex.Should().Be(0);
            sut.HasPlayerStats(0).Should().BeTrue();
        }
    }
}
