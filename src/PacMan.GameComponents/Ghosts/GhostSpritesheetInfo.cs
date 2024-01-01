﻿namespace PacMan.GameComponents.Ghosts;

public class GhostSpritesheetInfo
{
    private const int _width = 16;

    private readonly Dictionary<Direction, FramePair> _frames;

    public GhostSpritesheetInfo(Vector2 position)
    {
        _frames = new();

        var toMove = new Vector2(_width, 0);

        Vector2 marker = position;

        _frames[Direction.Right] = new(position, position + toMove);
        marker += toMove;
        marker += toMove;

        _frames[Direction.Left] = new(marker, marker + toMove);
        marker += toMove;
        marker += toMove;

        _frames[Direction.Up] = new(marker, marker + toMove);
        marker += toMove;
        marker += toMove;

        _frames[Direction.Down] = new(marker, marker + toMove);
    }

    public Vector2 GetSourcePosition(Direction direction, bool useFirstFrame)
    {
        FramePair frame = _frames[direction];

        return useFirstFrame ? frame.First : frame.Second;
    }
}