using PacMan.GameComponents.Ghosts;

namespace PacMan.GameComponents
{
    public class GlobalDotCounter : DotCounter
    {
        bool _finished;

        public GlobalDotCounter(int limit = 0) : base(limit, "GLOBAL")
        {
        }

        public virtual void Reset()
        {
            _finished = false;
            Counter = 0;
        }

        // the thing that calls this switches dot counters if all the ghosts are out.
        // this never finishes if Clyde is out the house when the counter reaches
        // 32 - this mimics the bug in the arcade game
        public override bool IsFinished => _finished;

        GhostNickname? _nextOneToForceOut;
        GhostNickname? _lastOneForcedOut;

        public override void SetTimedOut()
        {
            if (_lastOneForcedOut == null || _lastOneForcedOut == GhostNickname.Clyde)
            {
                _nextOneToForceOut = GhostNickname.Pinky;
            }
            else if (_lastOneForcedOut == GhostNickname.Pinky)
            {
                _nextOneToForceOut = GhostNickname.Inky;
            }
            else if (_lastOneForcedOut == GhostNickname.Inky)
            {
                _nextOneToForceOut = GhostNickname.Clyde;
            }
        }

        public bool CanGhostLeave(GhostNickname nickName)
        {
            if (_nextOneToForceOut == nickName)
            {
                _nextOneToForceOut = null;
                _lastOneForcedOut = nickName;
                return true;
            }

            bool canLeave = false;

            switch (nickName)
            {
                case GhostNickname.Pinky when Counter == 7:
                    canLeave = true;
                    break;
                case GhostNickname.Inky when Counter == 17:
                    canLeave = true;
                    break;
                case GhostNickname.Clyde when Counter == 32:
                    canLeave = true;
                    //debt: SD: confusing!
                    _finished = Counter == 32;
                    break;
            }

            return canLeave;
        }
    }
}