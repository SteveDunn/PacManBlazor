namespace PacMan.GameComponents;

public class FruitSession
{
    private int _toShowAt;
    private int _counter;

    public FruitSession()
    {
        _toShowAt = 70;
        _counter = 0;
    }

    public bool ShouldShow { get; private set; }

    public void PillEaten()
    {
        if (++_counter == _toShowAt)
        {
            ShouldShow = true;

            if (_toShowAt == 70)
            {
                _toShowAt = 170;
            }
            else
            {
                _toShowAt = -1;
            }
        }
        else
        {
            ShouldShow = false;
        }
    }

    public void FruitEaten()
    {
        ShouldShow = false;
    }
}