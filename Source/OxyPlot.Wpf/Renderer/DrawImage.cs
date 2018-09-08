namespace OxyPlot.Wpf
{
    /// <summary>
    /// Draw an image
    /// </summary>
    internal class DrawImage : ADrawOperation<DrawImage>
    {
        public DrawImage(
            OxyImage source,
            double srcX,
            double srcY,
            double srcWidth,
            double srcHeight,
            double destX,
            double destY,
            double destWidth,
            double destHeight,
            double opacity,
            bool interpolate)
        {
            this.Source = source;
            this.SrcX = srcX;
            this.SrcY = srcY;
            this.SrcWidth = srcWidth;
            this.SrcHeight = srcHeight;
            this.DestX = destX;
            this.DestY = destY;
            this.DestWidth = destWidth;
            this.DestHeight = destHeight;
            this.Opacity = opacity;
            this.Interpolate = interpolate;
        }

        public double DestHeight { get; }
        public double DestWidth { get; }
        public double DestX { get; }
        public double DestY { get; }
        public bool Interpolate { get; }
        public double Opacity { get; }
        public OxyImage Source { get; }
        public double SrcHeight { get; }
        public double SrcWidth { get; }
        public double SrcX { get; }
        public double SrcY { get; }

        public override bool Equals(DrawImage other)
        {
            return DestHeight == other.DestHeight
                && DestWidth == other.DestWidth
                && Interpolate == other.Interpolate
                && Opacity == other.Opacity
                && SrcHeight == other.SrcHeight
                && SrcWidth == other.SrcWidth
                && SrcX == other.SrcX
                && SrcY == other.SrcY
                && (Source == other.Source);
        }

        public override bool Transposed(DrawImage other)
        {
            return Equals(other);
        }
    }
}
