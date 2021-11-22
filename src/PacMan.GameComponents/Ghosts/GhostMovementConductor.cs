using PacMan.GameComponents.Canvas;

namespace PacMan.GameComponents.Ghosts;

public class GhostMovementConductor
{
    int _index;

    readonly List<ModeAndDuration> _items;

    [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
    public GhostMovementConductor(GhostsLevelPatternProperties properties)
    {
        _index = -1;

        _items = new() {
            new(GhostMovementMode.Scatter, properties.Scatter1.Seconds()),
            new(GhostMovementMode.Chase, properties.Chase1.Seconds()),
            new(GhostMovementMode.Scatter, properties.Scatter2.Seconds()),
            new(GhostMovementMode.Chase, properties.Chase2.Seconds()),
            new(GhostMovementMode.Scatter, properties.Scatter3.Seconds()),
            new(GhostMovementMode.Chase, properties.Chase3.Seconds()),
            new(GhostMovementMode.Scatter, properties.Scatter4.Seconds()),
            new(GhostMovementMode.Chase, properties.Chase4.Seconds())
        };

        incrementIndex();
    }

    public void Update(CanvasTimingInformation context)
    {
        ModeAndDuration item = _items[_index];

        item.DecreaseDurationBy(context.ElapsedTime);

        if (item.Duration < TimeSpan.Zero)
        {
            incrementIndex();
        }
    }

    void incrementIndex()
    {
        _index += 1;

        if (_index >= _items.Count)
        {
            throw new InvalidOperationException("No more move patterns!?");
        }
    }

    public GhostMovementMode CurrentMode => _items[_index].Mode;
}