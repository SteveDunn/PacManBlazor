using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;

namespace PacMan.GameComponents.Canvas
{
    public class CanvasWrapper
    {
        public static readonly CanvasTextFormat _10point = new("Assets/Joystix.ttf#Joystix", 10);
        public static readonly CanvasTextFormat _8point = new("Assets/Joystix.ttf#Joystix", 8);

        readonly Canvas2DContext _canvas2DContext;
        readonly Point _origin = new(0, 0);

        [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
        protected CanvasWrapper(Canvas2DContext canvas2DContext) =>
            _canvas2DContext = canvas2DContext ?? throw new InvalidOperationException("null canvas 2d context!");

        public CanvasWrapper(Canvas2DContext canvas2DContext, Point origin) : this(canvas2DContext)
        {
            _origin = origin;
        }

        [SuppressMessage("ReSharper", "HeapView.BoxingAllocation")]
        public async ValueTask DrawText(string text, Point pos, Color color)
        {
            Point p = pos;
            p.Offset(_origin);

            // todo: avoid heap allocation
            await _canvas2DContext.SetFillStyleAsync($"rgb({color.R},{color.G},{color.B})");

            await _canvas2DContext.FillTextAsync(text, p.X, p.Y);
        }

        public async ValueTask DrawText(
            string text,
            Point pos,
            Color color,
            CanvasTextFormat canvasTextFormat)
        {
            await _canvas2DContext.SetFontAsync(canvasTextFormat.FormattedString);

            await DrawText(text, pos, color);
        }

        public async ValueTask DrawFromOther(CanvasWrapper other, Point spritePosition, Rectangle sourceRect)
        {
            Point p = spritePosition;
            p.Offset(_origin);

            await _canvas2DContext.DrawImageAsync(
                other._canvas2DContext.Canvas,
                sourceRect.X,
                sourceRect.Y,
                sourceRect.Width,
                sourceRect.Height,
                p.X,
                p.Y,
                sourceRect.Width,
                sourceRect.Height);
        }

        public async ValueTask DrawImage(ElementReference spritesheet, Point spritePosition, Rectangle sourceRect)
        {
            Point p = spritePosition;
            p.Offset(_origin);

            await _canvas2DContext.DrawImageAsync(
                spritesheet,
                sourceRect.X,
                sourceRect.Y,
                sourceRect.Width,
                sourceRect.Height,
                p.X,
                p.Y,
                sourceRect.Width,
                sourceRect.Height);
        }

        public async ValueTask DrawImage(ElementReference spritesheet, Rectangle destRect, Rectangle sourceRect)
        {
            Point newPoint = destRect.Location;
            newPoint.Offset(_origin);

            var r = new Rectangle(newPoint, destRect.Size);

            await _canvas2DContext.DrawImageAsync(
                spritesheet,
                sourceRect.X,
                sourceRect.Y,
                sourceRect.Width,
                sourceRect.Height,
                r.X,
                r.Y,
                r.Width,
                r.Height);
        }

        public Task Clear(int x, int y, int width, int height) =>
            _canvas2DContext.ClearRectAsync(x, y, width, height);

        public Task Clear(int width, int height) =>
            _canvas2DContext.ClearRectAsync(0, 0, width, height);

        public async ValueTask FillRect(int x, int y, int width, int height, Color color)
        {
            await _canvas2DContext.SetFillStyleAsync(color.Name);

            await _canvas2DContext.FillRectAsync(x + _origin.X, y + _origin.Y, width, height);
        }

        public Task SetGlobalAlphaAsync(float f) => _canvas2DContext.SetGlobalAlphaAsync(f);

        [SuppressMessage("ReSharper", "HeapView.BoxingAllocation")]
        public async ValueTask DrawLine(Vector2 from, Vector2 to, Color color)
        {
            var f = from + _origin.ToVector2();
            var t = to + _origin.ToVector2();

            await _canvas2DContext.BeginPathAsync();
            await _canvas2DContext.SetStrokeStyleAsync($"rgb({color.R},{color.G},{color.B})");
            await _canvas2DContext.SetLineWidthAsync(3);
            await _canvas2DContext.SetLineCapAsync(LineCap.Round);

            await _canvas2DContext.MoveToAsync(f.X, f.Y);
            await _canvas2DContext.LineToAsync(t.X, t.Y);

            await _canvas2DContext.StrokeAsync();
        }

        public ValueTask DrawMyText(string text, Vector2 pos, Color color) => 
            DrawText(text, pos.ToPoint(), color, _10point);
    }
}