using PacMan.GameComponents.Tweening;

namespace PacMan.GameComponents.GameActs;

public class BlazorLogo
{
    readonly GeneralSprite _blazorLogo;
    readonly Tweener _colorTweener;

    public BlazorLogo()
    {
        _blazorLogo = new(
            new(90, 85),
            new(64, 60),
            Vector2.Zero,
            new(547, 160));

        var colorTweeningFunction = Tweener.CreateTweeningFunction<Linear>(Easing.EaseNone);

        _colorTweener = new(.12f, .8f, 3.Seconds(), colorTweeningFunction);
            
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