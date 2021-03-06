﻿namespace OxyPlot.Wpf
{
    internal class DrawText : ADrawOperation<DrawText>
    {
        public OxyColor Fill;
        public string FontFamily;
        public double FontSize;
        public double FontWeight;
        public HorizontalAlignment HAlign;
        public OxySize? MaxSize;
        public ScreenPoint Point;
        public double Rotate;
        public string Text;
        public VerticalAlignment VAlign;

        public DrawText(ScreenPoint p, string text, OxyColor fill,
            string fontFamily, double fontSize, double fontWeight,
            double rotate,
            HorizontalAlignment halign, VerticalAlignment valign,
            OxySize? maxSize)
        {
            Point = p;
            Text = text;
            Fill = fill;
            FontFamily = fontFamily;
            FontSize = fontSize;
            FontWeight = fontWeight;
            Rotate = rotate;
            HAlign = halign;
            VAlign = valign;
            MaxSize = maxSize;
        }

        public override bool Equals(DrawText other)
        {
            return Point.Equals(other.Point)
                && Equals(Text, other.Text)
                && Fill.Equals(other.Fill)
                && Equals(FontFamily, other.FontFamily)
                && FontSize == other.FontSize
                && FontWeight == other.FontWeight
                && Rotate == other.Rotate
                && VAlign == other.VAlign
                && HAlign == other.HAlign
                && Equals(MaxSize, other.MaxSize);
        }

        public override bool Transposed(DrawText other)
        {
            // The check for text equality is replaced with a check for text length
            // since we assume that the TextBlock size is not affected by the actual text.
            // This assumption could bite us in the ass...
            return Transposed(Point, other.Point)
                && (Text == other.Text || (Text != null && other.Text != null && Text.Length == other.Text.Length))
                && Fill.Equals(other.Fill)
                && Equals(FontFamily, other.FontFamily)
                && FontSize == other.FontSize
                && FontWeight == other.FontWeight
                && Rotate == other.Rotate
                && VAlign == other.VAlign
                && HAlign == other.HAlign
                && Equals(MaxSize, other.MaxSize);
        }
    }
}