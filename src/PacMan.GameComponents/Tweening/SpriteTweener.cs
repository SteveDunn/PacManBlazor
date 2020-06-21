using System;
using PacMan.GameComponents.Canvas;

namespace PacMan.GameComponents.Tweening
{
    /// <summary>
    /// Represents a type that wraps a tweener and calls the provided lambda on each tick.
    /// It stores the original color of the sprite so that the color's alpha can be blended against it.
    /// </summary>
    public class SpriteTweener
    {
        readonly GeneralSprite _sprite;
        readonly Tweener _tweener;
        readonly Action<SpriteTweener> _whenRunning;

        public SpriteTweener(
            GeneralSprite sprite,
            Tweener tweener,
            Action<SpriteTweener> whenRunning,
            Action<SpriteTweener> whenFinished)
        {
            _sprite = sprite;
            _tweener = tweener;
            _whenRunning = whenRunning;
            _tweener.Ended += () => whenFinished(this);
        }

        public GeneralSprite Sprite
        {
            get
            {
                return _sprite;
            }
        }

        public float Position
        {
            get
            {
                return _tweener.Position;
            }
        }

        public void Update(CanvasTimingInformation gameTime)
        {
            if (_tweener.Running)
            {
                _tweener.Update(gameTime);
                _whenRunning(this);
            }
        }
    }
}