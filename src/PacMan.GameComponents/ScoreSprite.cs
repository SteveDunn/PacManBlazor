using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace PacMan.GameComponents
{
    public class ScoreSprite : GeneralSprite
    {
        static readonly Dictionary<int, (Vector2 Pos, int Width)> _scorePositions =
            new Dictionary<int, (Vector2, int)>()
            {
                {100, (new Vector2(456, 148), 15)},
                {200, (new Vector2(457, 133), 15)},
                {300, (new Vector2(473, 148), 15)},
                {400, (new Vector2(473, 133), 15)},
                {500, (new Vector2(489, 148), 15)},
                {700, (new Vector2(505, 148), 15)},
                {800, (new Vector2(489, 133), 15)},
                {1000, (new Vector2(521, 148), 21)},
                {1600, (new Vector2(505, 133), 16)},
                {2000, (new Vector2(518, 164), 21)},
                {3000, (new Vector2(518, 180), 21)},
                {5000, (new Vector2(518, 196), 21)}
            };

        public ScoreSprite(Vector2 position, int amount) : base(
            position,
            new Size(_scorePositions[amount].Width, 7),
            new Vector2(_scorePositions[amount].Width / 2f, 7 / 2f),
            _scorePositions[amount].Pos)
        {
        }
    }
}