using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
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
        public void Defaulting()
        {
            IMediator mediator = new StubbedMediator();
            GameStats sut = new(mediator, new StubbedGameStorage());

            sut.AmountOfPlayers.Should().Be(0);
            sut.AnyonePlaying.Should().BeFalse();
            sut.IsDemo.Should().BeFalse();
            sut.IsGameOver.Should().BeTrue();
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
            
            // p1 eaten
            p1.DecreaseLives();
            sut.ChoseNextPlayer();
            
            var p2 = sut.CurrentPlayerStats;
            
            // p2 eaten
            p2.DecreaseLives();
            sut.ChoseNextPlayer();

            // we should be back to player 1
            sut.CurrentPlayerStats.Should().Be(p1);

            // p1 eaten
            p1.DecreaseLives();
            sut.ChoseNextPlayer();
            
            // we should be back to player 2
            sut.CurrentPlayerStats.Should().Be(p2);

            // p2 eaten
            p2.DecreaseLives();
            sut.ChoseNextPlayer();

            // p1 eaten
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
            
            // p1 eaten
            p1.DecreaseLives();
            sut.ChoseNextPlayer();
            
            var p2 = sut.CurrentPlayerStats;
            
            // p2 eaten
            p2.DecreaseLives();
            sut.ChoseNextPlayer();

            // p1 eaten
            p1.DecreaseLives();
            p1.LivesRemaining.Should().Be(1);
            sut.ChoseNextPlayer();
            
            // p2 eaten
            p2.DecreaseLives();
            sut.ChoseNextPlayer();
            
            // p1 eats enough pills to get an extra life
            // that's 10,000 points, so 1,000 pills @ 10 points each.
            for (int i = 0; i < 1_000; i++)
            {
                await p1.PillEaten(CellIndex.Zero);
            }

            p1.Score.Value.Should().Be(10_000);

            p1.LivesRemaining.Should().Be(2);
            
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

    public class StubbedGameStorage : IGameStorage
    {
        int _highScore = 10_000;

        public ValueTask<int> GetHighScore() => ValueTask.FromResult(_highScore);

        public ValueTask SetHighScore(int highScore)
        {
            _highScore = highScore;
            
            return ValueTask.CompletedTask;
        }
    }
}
