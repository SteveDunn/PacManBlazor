namespace PacMan.GameComponents.Canvas
{
    public class CanvasTextFormat
    {
        public CanvasTextFormat(string fontFamily, int fontSize)
        {
            FontFamily = fontFamily;
            FontSize = fontSize;
            
            // ReSharper disable once HeapView.BoxingAllocation
            FormattedString = $"{fontSize}px {fontFamily}";
        }

        public string FontFamily { get; }

        public int FontSize { get; }

        public string FormattedString { get; }
    }
}