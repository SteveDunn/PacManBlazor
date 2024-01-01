using System.Drawing;
using PacMan.GameComponents.Ghosts;

namespace PacMan.GameComponents;

public class AttractScenePacMan : ISprite
{
    private readonly Dictionary<Direction, FramePair> _velocitiesLookup;
    private readonly TwoFrameAnimation _animDirection;

    private Vector2 _frame1InSpriteMap;
    private Vector2 _frame2InSpriteMap;

    [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
    public AttractScenePacMan()
    {
        Visible = true;
        _animDirection = new(65.Milliseconds());

        Direction = Direction.Left;

        const float left = 456;
        const float left2 = 472;

        _velocitiesLookup = new() {
            [Direction.Up] = new(
                new(left, 32), new(left2, 32)),
            [Direction.Down] = new(
                new(left, 48), new(left2, 48)),
            [Direction.Left] = new(
                new(left, 16), new(left2, 16)),
            [Direction.Right] = new(
                new(left, 0), new(left2, 0))
        };

        Position = Tile.ToCenterCanvas(new(13.5f, 23));

        SetSpriteSheetPointers();
    }

    public Vector2 SpriteSheetPos { get; private set; }

    public bool Visible { get; set; }

    public Vector2 Position { get; set; }

    public ValueTask Draw(CanvasWrapper session)
    {
        return session.DrawSprite(this, Spritesheet.Reference);
    }

    public Size Size { get; } = new(16, 16);

    public Vector2 Origin => Vector2s.Eight;

    public Direction Direction { private get; set; }

    private ValueTask UpdateAnimation(CanvasTimingInformation context)
    {
        _animDirection.Run(context);

        SetSpriteSheetPointers();

        return default;
    }

    private void SetSpriteSheetPointers()
    {
        _frame1InSpriteMap = _velocitiesLookup[Direction].First;
        _frame2InSpriteMap = _velocitiesLookup[Direction].Second;

        SpriteSheetPos = _animDirection.Flag ? _frame1InSpriteMap : _frame2InSpriteMap;
    }

    public async ValueTask Update(CanvasTimingInformation timing)
    {
        await UpdateAnimation(timing);
    }
}