using System.Drawing;

namespace PacMan.GameComponents;

public class GeneralSprite : ISprite
{
    private readonly Vector2 _frame1;
    private readonly Vector2 _frame2;
    private readonly TwoFrameAnimation? _animator;
    private Vector2 _currentFrame;

    public GeneralSprite(
        Vector2 pos,
        Size size,
        Vector2 offsetForOrigin,
        Vector2 spritesheetPos)
    {
        _frame1 = spritesheetPos;

        Position = pos;
        Size = size;
        Origin = offsetForOrigin;

        Visible = true;

        _currentFrame = _frame1;
    }

    public GeneralSprite(
        Vector2 pos,
        Size size,
        Vector2 offsetForOrigin,
        Vector2 frame1,
        Vector2 frame2,
        TimeSpan animationSpeed) : this(pos, size, offsetForOrigin, frame1)
    {
        // ReSharper disable once HeapView.ObjectAllocation.Evident
        _animator = new(animationSpeed);
        _frame2 = frame2;
    }

    public Vector2 Position { get; set; }

    public ValueTask Update(CanvasTimingInformation timing)
    {
        if (_animator != null)
        {
            _animator.Run(timing);
            _currentFrame = _animator.Flag ? _frame2 : _frame1;
        }

        return default;
    }

    public ValueTask Draw(CanvasWrapper session) =>
        session.DrawSprite(this, Spritesheet.Reference);

    public Vector2 Origin { get; }

    public Size Size { get; }

    public Vector2 SpriteSheetPos => _currentFrame;

    public bool Visible { get; set; }
}