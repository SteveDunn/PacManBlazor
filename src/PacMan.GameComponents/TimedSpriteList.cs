namespace PacMan.GameComponents;

public class TimedSpriteList
{
    readonly List<TimedSprite> _sprites;

    public TimedSpriteList()
    {
        _sprites = new();
    }

    public void Add(TimedSprite sprite) => _sprites.Add(sprite);

    public void Update(CanvasTimingInformation timing)
    {
        foreach (var s in _sprites)
        {
            s.Update(timing);
        }

        for (int i = _sprites.Count - 1; i >= 0; i--)
        {
            if (_sprites[i].Expired)
            {
                _sprites.RemoveAt(i);
                break;
            }
        }
    }

    public async ValueTask Draw(CanvasWrapper session)
    {
        foreach (var s in _sprites)
        {
            await s.Draw(session);
        }
    }
}