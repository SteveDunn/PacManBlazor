using System.Drawing;
using System.Text;

namespace PacMan.GameComponents;

public class StatusPanel : IStatusPanel
{
    private readonly IGameStats _gameStats;
    private readonly ICoinBox _coinBox;

    // ReSharper disable once HeapView.ObjectAllocation.Evident
    private readonly StringBuilder _sb = new();

    private readonly Vector2 _creditTextPoint = new(10, 00);

    private readonly LoopingTimer _timer;

    private readonly SimpleFruit _fruit;

    private bool _tickTock = true;

    public StatusPanel(IGameStats gameStats, ICoinBox coinBox)
    {
        _gameStats = gameStats;
        _coinBox = coinBox;

        // ReSharper disable once HeapView.ObjectAllocation.Evident
        _timer = new(250.Milliseconds(), () => _tickTock = !_tickTock);

        // ReSharper disable once HeapView.ObjectAllocation.Evident
        _fruit = new();
    }

    public void Update(CanvasTimingInformation timing)
    {
        _timer.Run(timing);
    }

    public async ValueTask Draw(CanvasWrapper ds)
    {
        if (_gameStats.AnyonePlaying)
        {
            await DrawPlayerLives(ds);
            await DrawFruit(ds);
        }
        else
        {
            await DrawCredits(ds);
        }
    }

    private async ValueTask DrawPlayerLives(CanvasWrapper ds)
    {
        int x = 0;

        for (var i = 0; i < _gameStats.CurrentPlayerStats.Lives-1; i++, x += 16)
        {
            await ds.DrawImage(
                Spritesheet.Reference,
                new Rectangle(x, 0, 16, 16),
                new(
                    (int)PacMan.FacingLeftSpritesheetPos.X,
                    (int)PacMan.FacingLeftSpritesheetPos.Y,
                    16,
                    16));
        }
    }

    private ValueTask DrawCredits(CanvasWrapper ds)
    {
        _sb.Clear();
        return ds.DrawMyText(_sb.Append("CREDIT ").Append(_coinBox.Credits).ToString(), _creditTextPoint, Colors.White);
    }

    // drawSprite max 7 fruit from max level 21
    private async ValueTask DrawFruit(CanvasWrapper ds)
    {
        if (_gameStats.IsDemo)
        {
            return;
        }

        var highestLevel = Math.Min(
            20,
            _gameStats.CurrentPlayerStats.LevelStats.LevelNumber);

        var lowestLevel = Math.Max(
            0,
            highestLevel - 6);

        var x = 204;

        // starting from the right
        for (var i = lowestLevel; i <= highestLevel; i++, x -= 16)
        {
            var item = LevelStats.GetLevelProps(i).Fruit1;

            _fruit.SetFruitItem(item);
            _fruit.Position = new(x, 10);

            await ds.DrawSprite(_fruit, Spritesheet.Reference);
        }
    }
}