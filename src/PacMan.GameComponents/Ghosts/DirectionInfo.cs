namespace PacMan.GameComponents.Ghosts
{
    public class DirectionInfo
    {
        public DirectionInfo(Direction current, Direction next) => (Current, Next) = (current, next);

        public Direction Current { get; private set; }

        public Direction Next { get; private set; }

        public void Update(Direction nextDirection)
        {
            Current = Next;
            Next = nextDirection;
        }
    }
}