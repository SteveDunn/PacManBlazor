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
        public static void Deconstruct(this Vector2 v, out int x, out int y) => (x,y) = ((int)v.X, (int)v.Y);

        public static CellIndex ToCellIndex(this Vector2 vector2) => new((int) vector2.X, (int) vector2.Y);

        public static Vector2 ToVector2(this Point point) => new(point.X, point.Y);

        public static Vector2 ToVector2(this CellIndex cellIndex) => new(cellIndex.X, cellIndex.Y);

        public static Point ToPoint(this Vector2 v) => new((int) v.X, (int) v.Y);

        public static float DistanceBetween(Vector2 cell1, Vector2 cell2)
        {
            float a = cell1.X - cell2.X;
            float b = cell1.Y - cell2.Y;

            return (float) Math.Sqrt((a * a) + (b * b));
        }

        public static float DistanceBetween(CellIndex cell1, CellIndex cell2)
        {
            float a = cell1.X - cell2.X;
            float b = cell1.Y - cell2.Y;

            return (float) Math.Sqrt((a * a) + (b * b));
        }

        [JetBrains.Annotations.Pure]
        public static Vector2 Normalize(this Vector2 value)
        {
            float val = (float) (1.0 / Math.Sqrt((value.X * value.X) + (value.Y * value.Y)));

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

        public static async ValueTask DrawSprite(
            this CanvasWrapper session,
            ISprite sprite,
            ElementReference spriteSheet)
        {
            if (sprite.Visible)
            {
                await session.DrawImage(
                    spriteSheet,
                    (sprite.Position - sprite.Origin).toPoint(),
                    new(sprite.SpriteSheetPos.toPoint(), sprite.Size));
            }
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

        static Point toPoint(this Vector2 vector) => new((int) vector.X, (int) vector.Y);

        public static bool Intersects(this Rectangle r1, Rectangle r2) =>
            !(r2.Left > r1.Right ||
              r2.Right < r1.Left ||
              r2.Top > r1.Bottom ||
              r2.Bottom < r1.Top);

        [JetBrains.Annotations.Pure]
        public static Rectangle Expand(this Rectangle r, double amount) =>
            new(
                new((int) (r.Left - amount), (int) (r.Top - amount)),
                new((int) (r.Width + amount), (int) (r.Height + amount)));

        public static Vector2 Floor(this Vector2 point) =>
            new((float) Math.Floor(point.X), (float) Math.Floor(point.Y));

        public static Vector2 Round(this Vector2 v) => new((float) Math.Round(v.X), (float) Math.Round(v.Y));
    }
}