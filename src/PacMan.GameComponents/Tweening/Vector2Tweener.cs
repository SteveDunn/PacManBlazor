using System;
using System.Numerics;
using PacMan.GameComponents.Canvas;

namespace PacMan.GameComponents.Tweening
{
    public class Vector2Tweener
    {
        readonly Tweener _tweenerX;
        readonly Tweener _tweenerY;

        public delegate void EndHandler();

        public event TweenEndHandler? Ended;

        public Vector2Tweener(Vector2 start, Vector2 end, Type type, Easing easing, TimeSpan duration)
        {
            _tweenerX = new(start.X, end.X, duration, Tweener.CreateTweeningFunction(type, easing));
            _tweenerY = new(start.Y, end.Y, duration, Tweener.CreateTweeningFunction(type, easing));
            
            _tweenerX.Ended += () =>
            {
                if (_tweenerY.HasEnded)
                {
                    Ended?.Invoke();
                }
            };

            _tweenerY.Ended += () =>
            {
                if (_tweenerX.HasEnded)
                {
                    Ended?.Invoke();
                }
            };
        }

        public Vector2 Position
        {
            get
            {
                return new(_tweenerX.Position, _tweenerY.Position);
            }
        }

        public void Update(CanvasTimingInformation gameTime)
        {
            _tweenerX.Update(gameTime);
            _tweenerY.Update(gameTime);
        }

        public static Vector2Tweener Create<T>(Vector2 start, Vector2 end, Easing easing, TimeSpan duration)
        {
            return Create(start, end, typeof(T), easing, duration);
        }

        public static Vector2Tweener Create(Vector2 start, Vector2 end, Type type, Easing easing, TimeSpan duration)
        {
            return new(start, end, type, easing, duration);
        }

        public static Vector2Tweener Create(Vector2 start, Vector2 end, TweenCreationSettings tweenCreationSettings)
        {
            return Create(start, end, tweenCreationSettings.Type, tweenCreationSettings.Easing, tweenCreationSettings.Duration);
        }
    }
}