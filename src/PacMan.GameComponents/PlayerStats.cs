using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MediatR;
using PacMan.GameComponents.Canvas;
using PacMan.GameComponents.Events;
using PacMan.GameComponents.Ghosts;

namespace PacMan.GameComponents
{
    public class PlayerStats
    {
        readonly IMediator _mediator;
        GhostHouseDoor _ghostHouseDoor;
        LevelStats _levelStats;
        int _levelNumber;

        GhostMovementConductor _ghostMovementConductor;

        readonly List<int> _extraLives;

        [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
        public PlayerStats(int playerIndex, IMediator mediator)
        {
            _mediator = mediator;
            PlayerIndex = playerIndex;

            Score = 0;

            // cheat
            LivesRemaining = Constants.PacManLives;
            _levelNumber = -1;

            _extraLives = new List<int> { 10000 };
            _levelStats = new LevelStats(0);
            _ghostHouseDoor = new GhostHouseDoor(0, _mediator);

            var props = LevelStats.GetGhostPatternProperties();

            _ghostMovementConductor = new GhostMovementConductor(props);
        }

        public void Update(CanvasTimingInformation timing)
        {
            if (FrightSession != null && !FrightSession.IsFinished)
            {
                FrightSession.Update(timing);
            }
            else
            {
                _ghostMovementConductor.Update(timing);
            }

            _ghostHouseDoor.Update(timing);
        }

        public GhostMovementConductor ghostMoveConductor => _ghostMovementConductor;

        public GhostHouseDoor ghostHouseDoor => _ghostHouseDoor;

        [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
        public void NewLevel()
        {
            _levelStats = new LevelStats(++_levelNumber);
            _ghostHouseDoor = new GhostHouseDoor(0, _mediator);

            var props = LevelStats.GetGhostPatternProperties();

            _ghostMovementConductor = new GhostMovementConductor(props);
        }

        public int PlayerIndex { get; }

        public LevelStats LevelStats => _levelStats;

        public int LivesRemaining { get; protected set; }

        protected async virtual ValueTask IncreaseScoreBy(int amount)
        {
            Score += amount;

            if (_extraLives.Count == 0)
            {
                return;
            }

            if (Score > _extraLives[0])
            {
                await _mediator.Publish(new ExtraLifeEvent());

                LivesRemaining += 1;

                _extraLives.RemoveAt(0);
            }
        }

        public async ValueTask PillEaten(CellIndex point)
        {
            await _ghostHouseDoor.PillEaten();
            await IncreaseScoreBy(10);
            _levelStats.PillEaten(point);
        }

        public async ValueTask FruitEaten()
        {
            await IncreaseScoreBy(_levelStats.GetLevelProps().FruitPoints);
            LevelStats.FruitSession.FruitEaten();
        }

        public GhostFrightSession? FrightSession { get; private set; }

        public bool IsInFrightSession => FrightSession != null && !FrightSession.IsFinished;

        public async ValueTask PowerPillEaten(CellIndex point)
        {
            FrightSession = new GhostFrightSession(_levelStats.GetLevelProps());

            await _ghostHouseDoor.PillEaten();
            await IncreaseScoreBy(50);
            _levelStats.PillEaten(point);
        }

        public void PacManEaten()
        {
            _ghostHouseDoor.SwitchToUseGlobalCounter();

            var props = LevelStats.GetGhostPatternProperties();

            _ghostMovementConductor = new GhostMovementConductor(props);
        }

        public void DecreaseLives()
        {
            LivesRemaining -= 1;
        }

        public async ValueTask<int> GhostEaten()
        {
            if (FrightSession == null)
            {
                throw new InvalidOperationException("ghost can't be eaten as there's no fright session");
            }

            var points = FrightSession.GhostEaten();

            await IncreaseScoreBy(points);

            return points;
        }

        public int Score { get; private set; }
    }
}