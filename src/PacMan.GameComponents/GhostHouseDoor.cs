using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MediatR;
using PacMan.GameComponents.Canvas;
using PacMan.GameComponents.Ghosts;
using PacMan.GameComponents.Requests;

namespace PacMan.GameComponents
{
    public class GhostHouseDoor
    {
        readonly IMediator _mediator;

        readonly Dictionary<GhostNickname, DotCounter> _ghostCounters;
        readonly GlobalDotCounter _globalCounter;
        readonly DotCounter _nullCounter;

        DotCounter _activeCounter;
        TimeSpan _pillConsumptionTimeIdle;

        [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
        public GhostHouseDoor(int level, IMediator mediator)
        {
            _mediator = mediator;

            _ghostCounters = new Dictionary<GhostNickname, DotCounter>();

            _nullCounter = new DotCounter(int.MaxValue, "NULL");

            _globalCounter = new GlobalDotCounter();

            var pinkyCounter = new DotCounter(0, "PINKY");

            if (level == 0)
            {
                _ghostCounters[GhostNickname.Pinky] = pinkyCounter;
                _ghostCounters[GhostNickname.Inky] = new DotCounter(30, "INKY");
                _ghostCounters[GhostNickname.Clyde] = new DotCounter(60, "CLYDE");
            }

            if (level == 1)
            {
                _ghostCounters[GhostNickname.Pinky] = pinkyCounter;
                _ghostCounters[GhostNickname.Inky] = new DotCounter(0, "INKY");
                _ghostCounters[GhostNickname.Clyde] = new DotCounter(50, "CLYDE");
            }

            if (level >= 2)
            {
                _ghostCounters[GhostNickname.Pinky] = pinkyCounter;
                _ghostCounters[GhostNickname.Inky] = new DotCounter(0, "INKY");
                _ghostCounters[GhostNickname.Clyde] = new DotCounter(0, "CLYDE");
            }

            _pillConsumptionTimeIdle = TimeSpan.Zero;

            _activeCounter = pinkyCounter;

            _activeCounter.Activate();

            switchToUseCounterOfNextGhost();
        }

        public void Update(CanvasTimingInformation context)
        {
            _pillConsumptionTimeIdle += context.ElapsedTime;

            if (_pillConsumptionTimeIdle.TotalMilliseconds > 4000)
            {
                whenNoPillsEaten();
            }
        }

        void whenNoPillsEaten()
        {
            _pillConsumptionTimeIdle = TimeSpan.Zero;
            _activeCounter.SetTimedOut();

            switchToUseCounterOfNextGhost();
        }

        void switchToUseCounterOfNextGhost()
        {
            if (_activeCounter == _globalCounter)
            {
                if (_activeCounter.IsFinished)
                {
                    switchActive(_nullCounter);
                }

                return;
            }

            if (_activeCounter == _nullCounter)
            {
                return;
            }

            if (_activeCounter == _ghostCounters[GhostNickname.Pinky])
            {
                switchActive(_ghostCounters[GhostNickname.Inky]);
            }
            else if (_activeCounter == _ghostCounters[GhostNickname.Inky])
            {
                switchActive(_ghostCounters[GhostNickname.Clyde]);
            }
            else if (_activeCounter == _ghostCounters[GhostNickname.Clyde])
            {
                switchActive(_nullCounter);
            }
            else
            {
                // ReSharper disable once HeapView.ObjectAllocation.Evident
                throw new InvalidOperationException("don't know where to switch the active dot counter to!");
            }
        }

        public bool CanGhostLeave(Ghost ghost)
        {
            if (ghost.NickName == GhostNickname.Blinky)
            {
                return true;
            }

            if (_globalCounter.IsActive)
            {
                return _globalCounter.CanGhostLeave(ghost.NickName);
            }

            if (_activeCounter == _nullCounter)
            {
                return true;
            }

            return _ghostCounters[ghost.NickName].LimitReached;
        }

        public void SwitchToUseGlobalCounter()
        {
            _globalCounter.Reset();

            switchActive(_globalCounter);
        }

        void switchActive(DotCounter counter)
        {
            _activeCounter.Deactivate();

            _activeCounter = counter;

            _activeCounter.Activate();
        }

        public async ValueTask PillEaten()
        {
            _pillConsumptionTimeIdle = TimeSpan.Zero;

            // from: https://www.gamasutra.com/view/feature/132330/the_pacman_dossier.php?page=4
            // The three ghosts inside the house must wait for this special counter to tell them when
            // to leave. Pinky is released when the counter value is equal to 7 and Inky is released
            // when it equals 17. The only way to deactivate the counter is for Clyde to be inside
            // the house when the counter equals 32; otherwise, it will keep counting dots even
            // after the ghost house is empty.
            // If Clyde is present at the appropriate time, the global counter is reset to zero and
            // deactivated, and the ghosts' personal dot limits are re-enabled and used as before for
            // determining when to leave the house (including Clyde who is still in the house at this time).
            if (_activeCounter == _globalCounter)
            {
                if (_globalCounter.Counter == 32)
                {
                    // ReSharper disable once HeapView.BoxingAllocation
                    var state = await _mediator.Send(new GetGameStateRequest());

                    if (state.IsClydeInHouse)
                    {
                        Console.WriteLine(
                            "** Clyde is in the house and the global counter is 32 - honoring the original bug by deactivating the global counter");

                        switchActive(_nullCounter);
                    }
                }
            }

            _activeCounter.Increment();

            if (_activeCounter.LimitReached)
            {
                switchToUseCounterOfNextGhost();
            }
        }
    }
}