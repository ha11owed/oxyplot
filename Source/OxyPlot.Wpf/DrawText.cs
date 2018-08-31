// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CanvasRenderContext.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Implements <see cref="IRenderContext" /> for <see cref="System.Windows.Controls.Canvas" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    internal class DrawText : ADrawOperation<DrawText>
    {
        private OxyColor fill;
        private string fontFamily;
        private double fontSize;
        private double fontWeight;
        private HorizontalAlignment halign;
        private OxySize? maxSize;
        private ScreenPoint p;
        private double rotate;
        private string text;
        private VerticalAlignment valign;

        public DrawText(ScreenPoint p, string text, OxyColor fill,
            string fontFamily, double fontSize, double fontWeight,
            double rotate,
            HorizontalAlignment halign, VerticalAlignment valign,
            OxySize? maxSize)
        {
            this.p = p;
            this.text = text;
            this.fill = fill;
            this.fontFamily = fontFamily;
            this.fontSize = fontSize;
            this.fontWeight = fontWeight;
            this.rotate = rotate;
            this.halign = halign;
            this.valign = valign;
            this.maxSize = maxSize;
        }

        public override bool Transposed(DrawText other)
        {
            return Transposed(p, other.p)
                && Equals(text, other.text)
                && Equals(fill, other.fill)
                && Equals(fontFamily, other.fontFamily)
                && Equals(fontSize, other.fontSize)
                && Equals(fontWeight, other.fontWeight)
                && rotate == other.rotate
                && valign == other.valign
                && halign == other.halign
                && Equals(maxSize, other.maxSize);
        }
    }
}
