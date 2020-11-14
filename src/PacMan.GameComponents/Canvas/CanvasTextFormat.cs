namespace PacMan.GameComponents.Canvas
{
public record CanvasTextFormat(string FontFamily, int FontSize)
{
    public readonly string FormattedString =  $"{FontSize}px {FontFamily}";
}
}