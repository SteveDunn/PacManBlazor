using System;
using System.Drawing;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using PacMan.GameComponents.Canvas;

namespace PacMan.GameComponents
{
    public class StatusPanel : IStatusPanel
    {
        readonly IGameStats _gameStats;
        readonly ICoinBox _coinBox;

        // ReSharper disable once HeapView.ObjectAllocation.Evident
        readonly StringBuilder _sb = new();

        readonly Vector2 _creditTextPoint = new(10, 00);

        readonly LoopingTimer _timer;

        readonly SimpleFruit _fruit;

        bool _tickTock = true;

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
                await drawPlayerLives(ds);
                await drawFruit(ds);
            }
            else
            {
                await drawCredits(ds);
            }
        }

        async ValueTask drawPlayerLives(CanvasWrapper ds)
        {
            int x = 0;

            for (var i = 0; i < _gameStats.CurrentPlayerStats.LivesRemaining; i++, x += 16)
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

        ValueTask drawCredits(CanvasWrapper ds)
        {
            _sb.Clear();
            return ds.DrawMyText(_sb.Append("CREDIT ").Append(_coinBox.Credits).ToString(), _creditTextPoint, Colors.White);
        }

        // drawSprite max 7 fruit from max level 21
        async ValueTask drawFruit(CanvasWrapper ds)
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
}