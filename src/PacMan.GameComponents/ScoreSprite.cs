using System.Collections.Generic;
using System.Numerics;

namespace PacMan.GameComponents;

public class ScoreSprite : GeneralSprite
{
    static readonly Dictionary<int, (Vector2 Pos, int Width)> _scorePositions =
        new()
        {
            { 100, (new(456, 148), 15) },
            { 200, (new(457, 133), 15) },
            { 300, (new(473, 148), 15) },
            { 400, (new(473, 133), 15) },
            { 500, (new(489, 148), 15) },
            { 700, (new(505, 148), 15) },
            { 800, (new(489, 133), 15) },
            { 1000, (new(521, 148), 21) },
            { 1600, (new(505, 133), 16) },
            { 2000, (new(518, 164), 21) },
            { 3000, (new(518, 180), 21) },
            { 5000, (new(518, 196), 21) }
        };

    public ScoreSprite(Vector2 position, int amount) : base(
        position,
        new(_scorePositions[amount].Width, 7),
        new(_scorePositions[amount].Width / 2f, 7 / 2f),
        _scorePositions[amount].Pos)
    {
    }
}