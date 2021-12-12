namespace PacMan.GameComponents.Canvas;

public record CanvasTextFormat(string FontFamily, int FontSize)
{
    // ReSharper disable once HeapView.BoxingAllocation
    public readonly string FormattedString =  $"{FontSize}px {FontFamily}";
}