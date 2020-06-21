using PacMan.GameComponents.Ghosts;

namespace PacMan.GameComponents
{
    public class AttractGhost : SimpleGhost
    {
        public AttractGhost(GhostNickname nickName, Directions direction) : base(nickName, direction)
        {
            Alive = true;
        }

        public void SetFrightened()
        {
            State = GhostState.Frightened;
        }

        public bool Alive { get; set; }
    }
}