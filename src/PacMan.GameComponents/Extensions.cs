using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using PacMan.GameComponents.Canvas;

namespace PacMan.GameComponents
{
    public static class Extensions
    {
        public static CellIndex ToCellIndex(this Vector2 vector2) => new CellIndex((int) vector2.X, (int) vector2.Y);

        public static Vector2 ToVector2(this Point point) => new Vector2(point.X,point.Y);
        
        public static Vector2 ToVector2(this CellIndex cellIndex) => new Vector2(cellIndex.X,cellIndex.Y);
        
        public static Point ToPoint(this Vector2 v) => new Point((int) v.X,(int) v.Y);

        static readonly CanvasTextFormat _canvasTextFormat = new CanvasTextFormat("Assets/Joystix.ttf#Joystix", 10);

        public static float DistanceBetween(Vector2 cell1, Vector2 cell2)
        {
            float a = cell1.X - cell2.X;
            float b = cell1.Y - cell2.Y;

            return (float)Math.Sqrt(a * a + b * b);
        }

        public static float DistanceBetween(CellIndex cell1, CellIndex cell2)
        {
            float a = cell1.X - cell2.X;
            float b = cell1.Y - cell2.Y;

            return (float)Math.Sqrt(a * a + b * b);
        }

        [JetBrains.Annotations.Pure]
        public static Vector2 Normalize(this Vector2 value)
        {
            float val = (float)(1.0 / Math.Sqrt(value.X * value.X + value.Y * value.Y));

            value.X *= val;
            value.Y *= val;

            return value;
        }

        // public static async ValueTask DrawSprite(this ISprite sprite, ElementReference spriteSheet)
        // {
        //     if (sprite.Visible)
        //     {
        //         spr
        //         await DrawImage(
        //             spriteSheet,
        //             (sprite.Position - sprite.Origin).ToPoint(),
        //             new Rectangle(sprite.SpriteSheetPos.ToPoint(), sprite.Size));
        //     }
        // }

        public static async ValueTask DrawSprite(this CanvasWrapper session, 
            ISprite sprite, 
            ElementReference spriteSheet)
        {
            if (sprite.Visible)
            {
                await session.DrawImage(
                    spriteSheet,
                    (sprite.Position - sprite.Origin).toPoint(),
                    new Rectangle(sprite.SpriteSheetPos.toPoint(), sprite.Size));
            }
        }

        public static async ValueTask DrawMyText(this CanvasWrapper session, string text, Vector2 pos,
            Color color)
        {
            await session.DrawText(text, pos.toPoint(), color, _canvasTextFormat);
        }

        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (T item in items)
            {
                action(item);
            }
        }

        public static TimeSpan Milliseconds(this int n) => TimeSpan.FromMilliseconds(n);

        public static TimeSpan Seconds(this int n) => TimeSpan.FromSeconds(n);
        public static TimeSpan Seconds(this float n) => TimeSpan.FromSeconds(n);

        static Point toPoint(this Vector2 vector) => new Point((int) vector.X, (int) vector.Y);

        public static bool Intersects(this Rectangle r1, Rectangle r2) => !(r2.Left > r1.Right ||
                                                                            r2.Right < r1.Left ||
                                                                            r2.Top > r1.Bottom ||
                                                                            r2.Bottom < r1.Top);

        [JetBrains.Annotations.Pure]
        public static Rectangle Expand(this Rectangle rectangle, double amount) => new Rectangle(
            new Point((int) (rectangle.Left - amount), (int) (rectangle.Top - amount)),
            new Size((int) (rectangle.Width + amount), (int) (rectangle.Height + amount)));

        public static Vector2 Floor(this Vector2 point) => new Vector2((float)Math.Floor(point.X), (float)Math.Floor(point.Y));


        public static Vector2 Round(this Vector2 v) => new Vector2((float)Math.Round(v.X), (float)Math.Round(v.Y));
    }
}