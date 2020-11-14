using System.Threading.Tasks;

namespace PacMan.GameComponents.Ghosts
{
    /// Moves the ghost in a psuedo-random fashion while they are 'frightened' (i.e. blue)
    public class GhostFrightenedMover : GhostMover
    {
        public GhostFrightenedMover(Ghost ghost, IMaze maze) : base(ghost, GhostMovementMode.Frightened, maze, _getChaseTargetCell)
        {
        }

        static ValueTask<CellIndex> _getChaseTargetCell()
        {
            var random = Pnrg.Value;

            var cell = (random % 4) switch
            {
                0 => new CellIndex((int)MazeBounds.TopLeft.X, (int)MazeBounds.TopLeft.Y),
                1 => new CellIndex(MazeBounds.Dimensions.Width, 0),
                2 => new CellIndex(MazeBounds.Dimensions.Width, MazeBounds.Dimensions.Height),
                _ => new CellIndex(0, MazeBounds.Dimensions.Height)
            };

            return new ValueTask<CellIndex>(cell);
        }
    }
}