namespace PacMan.GameComponents;

public class Fruit : SimpleFruit, IFruit
{
    private readonly IMediator _mediator;
    private readonly IPacMan _pacman;
    private EggTimer _showTimer = EggTimer.Unset;
    private PlayerStats? _playerStats;
    private bool _isDemo;

    public Fruit(IMediator mediator, IPacMan pacman)
    {
        _mediator = mediator;
        _pacman = pacman;

        Reset();
    }

    [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
    private void Reset()
    {
        _showTimer = new(10.Seconds(), () => { Visible = false; });

        Position = Tile.ToCenterCanvas(new(14, 17.2f));

        Visible = false;
    }

    public async virtual ValueTask Update(CanvasTimingInformation timing)
    {
        if (Visible)
        {
            _showTimer.Run(timing);

            if (Vector2s.AreNear(_pacman.Position, Position, 4))
            {
                await _mediator.Publish(new FruitEatenEvent(this));

                Visible = false;
            }

            return;
        }

        if (_playerStats == null)
        {
            throw new InvalidOperationException("no player stats set!");
        }

        var levelStats = _playerStats.LevelStats;

        if (levelStats.FruitSession.ShouldShow && !_isDemo)
        {
            Visible = true;

            _showTimer.Reset();
        }

        SetFruitItem(levelStats.GetLevelProps().Fruit1);
    }

    public void HandlePlayerStarted(PlayerStats playerStats, bool isDemo)
    {
        _playerStats = playerStats;
        _isDemo = isDemo;
        Reset();
    }
}