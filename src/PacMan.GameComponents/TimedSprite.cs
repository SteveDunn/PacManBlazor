namespace PacMan.GameComponents;

public class TimedSprite
{
    private double _timeToLive;

    private readonly ISprite _sprite;

    public TimedSprite(int timeToLive, ISprite sprite)
    {
        _timeToLive = timeToLive;
        _sprite = sprite;
    }

    public void Update(CanvasTimingInformation timing)
    {
        _timeToLive -= timing.ElapsedTime.TotalMilliseconds;
    }

    public bool Expired => _timeToLive < 0;

    public async ValueTask Draw(CanvasWrapper session)
    {
        await _sprite.Draw(session);
    }
}