namespace PacMan.GameComponents;

public class DotCounter
{
    private readonly string _name;
    private readonly int _limit;

    private bool _timedOut;

    public DotCounter(int limit, string name)
    {
        _name = name;
        _limit = limit;
        _timedOut = false;

        IsActive = false;
        Counter = 0;
    }

    public bool IsActive { get; private set; }

    [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
    public void Increment()
    {
        if (!IsActive)
        {
            throw new InvalidOperationException($"Cannot increment a non active counter for {_name}");
        }

        if (Counter >= _limit && _limit > 0)
        {
            throw new InvalidOperationException($"Cannot increment counter, already at limit for {_name}.");
        }

        ++Counter;
    }

    public bool LimitReached => Counter == _limit || _timedOut;

    public int Counter { get; protected set; }

    public virtual void SetTimedOut() => _timedOut = true;

    public virtual bool IsFinished => false;

    public void Deactivate() => IsActive = false;

    public void Activate() => IsActive = true;
}