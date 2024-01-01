namespace PacMan.GameComponents;

public class TimerList
{
    private readonly List<EggTimer> _timers;

    [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
    public TimerList() => _timers = new();

    public void Add(EggTimer timer) => _timers.Add(timer);

    public void Update(CanvasTimingInformation timing)
    {
        foreach (var s in _timers)
        {
            s.Run(timing);
        }

        for (var i = _timers.Count - 1; i >= 0; i--)
        {
            EggTimer eggTimer = _timers[i];

            if (eggTimer.Finished)
            {
                _timers.RemoveAt(i);

                break;
            }
        }
    }
}