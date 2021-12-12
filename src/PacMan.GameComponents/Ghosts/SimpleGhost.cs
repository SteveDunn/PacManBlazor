using System.Drawing;

namespace PacMan.GameComponents.Ghosts;

public class SimpleGhost : ISprite
{
    protected readonly GhostSpritesheetInfo SpritesheetInfoNormal;

    readonly FrightenedSpritesheet _spritesheetInfoFrightened;
    readonly EyesSpritesheetInfo _spriteSheetEyes;

    readonly TwoFrameAnimation _toggle = new(65.Milliseconds());

    GhostFrightSession? _frightSession;

    [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
    public SimpleGhost(GhostNickname nickName, Direction direction)
    {
        NickName = nickName;
        Direction = new(direction, direction);

        Visible = true;

        var spriteSheet = new GhostSpritesheet();

        SpritesheetInfoNormal = spriteSheet.GetEntry(nickName);
        _spritesheetInfoFrightened = new GhostSpritesheet().GetFrightened();
        _spriteSheetEyes = spriteSheet.Eyes;

        SpriteSheetPos = SpritesheetInfoNormal.GetSourcePosition(Direction.Next, true);
    }

    public GhostNickname NickName { get; }

    public DirectionInfo Direction { get; set; }

    public bool Visible { get; set; }

    public Size Size => new(16, 16);

    protected GhostMovementMode MovementMode { get; set; }

    public GhostState State { get; protected set; }

    public bool IsInHouse => MovementMode == GhostMovementMode.InHouse;

    public Vector2 SpriteSheetPos { get; protected set; }

    public Vector2 Origin => Vector2s.Eight;

    public virtual Vector2 Position { get; set; }

    public void SetFrightSession(GhostFrightSession value)
    {
        _frightSession = value ?? throw new ArgumentNullException(nameof(value));
    }

    public virtual ValueTask Update(CanvasTimingInformation timing)
    {
        updateAnimation(timing);
        return default;
    }

    public virtual ValueTask Draw(CanvasWrapper session)
    {
        if (Visible)
        {
            return session.DrawSprite(this, Spritesheet.Reference);
        }

        return default;
    }

    void updateAnimation(CanvasTimingInformation context)
    {
        _toggle.Run(context);

        if (State == GhostState.Frightened)
        {
            SpriteSheetPos = getGhostFrame();
        }
        else if (State == GhostState.Eyes)
        {
            SpriteSheetPos = _spriteSheetEyes.GetSourcePosition(Direction.Next);
        }
        else
        {
            SpriteSheetPos = SpritesheetInfoNormal.GetSourcePosition(Direction.Next, _toggle.Flag);
        }
    }

    public virtual void PowerPillEaten(GhostFrightSession session) => SetFrightSession(session);

    Vector2 getGhostFrame()
    {
        if (_frightSession == null)
        {
            throw new InvalidOperationException("Cannot get ghost frame - not in a fright session");
        }

        FramePair pair = _frightSession.IsWhite
            ? _spritesheetInfoFrightened.White
            : _spritesheetInfoFrightened.Blue;

        return _toggle.Flag ? pair.First : pair.Second;
    }
}