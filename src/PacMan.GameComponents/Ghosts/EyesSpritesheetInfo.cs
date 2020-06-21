using System.Collections.Generic;
using System.Numerics;

namespace PacMan.GameComponents.Ghosts
{
    public class EyesSpritesheetInfo
    {
        readonly Dictionary<Directions, Vector2> _positions;

        const int _width = 16;

        public EyesSpritesheetInfo(Vector2 position)
        {
            _positions = new Dictionary<Directions, Vector2>();

            var toMove = new Vector2(_width, 0);

            _positions[Directions.Right] = position;

            var marker = position;
            marker = marker + toMove;

            _positions[Directions.Left] = marker;
            marker = marker + toMove;

            _positions[Directions.Up] = marker;
            marker = marker + toMove;

            _positions[Directions.Down] = marker;
        }

        public Vector2 GetSourcePosition(Directions direction) => _positions[direction];
    }
}