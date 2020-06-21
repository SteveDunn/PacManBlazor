using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using PacMan.GameComponents.Canvas;
using PacMan.GameComponents.Tweening;

namespace PacMan.GameComponents.GameActs
{
    public class BlazorLogo
    {
        readonly GeneralSprite _blazorLogo;
        readonly Tweener _colorTweener;


        public BlazorLogo()
        {
            _blazorLogo = new GeneralSprite(
                new Vector2(90, 85),
                new Size(64, 60),
                Vector2.Zero,
                new Vector2(547, 160));



            var colorTweeningFunction = Tweener.CreateTweeningFunction<Linear>(Easing.EaseNone);


            _colorTweener=new Tweener(.12f,.8f,3.Seconds(), colorTweeningFunction);
            _colorTweener.Ended += () =>
            {
                _colorTweener.Reverse();
                _colorTweener.Reset();
            };
        }

        public ValueTask<ActUpdateResult> Update(CanvasTimingInformation timing)
        {
             _colorTweener.Update(timing);

             return default;
        }

        public async ValueTask Draw(CanvasWrapper session)
        {
            await session.SetGlobalAlphaAsync(_colorTweener.Position);
            await session.DrawSprite(_blazorLogo, Spritesheet.Reference);
            await session.SetGlobalAlphaAsync(1f);
        }
    }
}