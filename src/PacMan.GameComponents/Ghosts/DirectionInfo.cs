namespace PacMan.GameComponents.Ghosts
{
    public class DirectionInfo
    {
        public DirectionInfo(Directions current, Directions next) => (Current, Next) = (current, next);

        public Directions Current { get; private set; }

        public Directions Next { get; private set; }

        public void Update(Directions nextDirection)
        {
            Current = Next;
            Next = nextDirection;
        }
    }
}