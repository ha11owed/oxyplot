using System;

namespace OxyPlot.Wpf
{
    internal class DrawImage : ADrawOperation<DrawImage>
    {
        public DrawImage(OxyImage source,
            double srcX, double srcY,
            double srcWidth, double srcHeight,
            double destX, double destY,
            double destWidth, double destHeight,
            double opacity, bool interpolate)
        {
            Source = source;
            SrcX = srcX;
            SrcY = srcY;
            SrcWidth = srcWidth;
            SrcHeight = srcHeight;
            DestX = destX;
            DestY = destY;
            DestWidth = destWidth;
            DestHeight = destHeight;
            Opacity = opacity;
            Interpolate = interpolate;
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

        public override bool Transposed(DrawImage other)
        {
            return Equals(DestHeight, other.DestHeight)
                && Equals(DestWidth, other.DestWidth)
                && Equals(Interpolate, other.Interpolate)
                && Equals(Opacity, other.Opacity)
                && Equals(SrcHeight, other.SrcHeight)
                && Equals(SrcWidth, other.SrcWidth)
                && Equals(SrcX, other.SrcX)
                && Equals(SrcY, other.SrcY)
                && Equals(Source, other.Source);
        }
    }
}
