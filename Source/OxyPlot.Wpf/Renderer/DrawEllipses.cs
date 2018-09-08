using System.Collections.Generic;

namespace OxyPlot.Wpf
{
    internal class DrawEllipses : ADrawOperation<DrawEllipses>
    {
        public DrawEllipses(IList<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness)
        {
            this.Rectangles = rectangles;
            this.Fill = fill;
            this.Stroke = stroke;
            this.Thickness = thickness;
        }

        public OxyColor Fill { get; }
        public IList<OxyRect> Rectangles { get; }
        public OxyColor Stroke { get; }
        public double Thickness { get; }

        public override bool Equals(DrawEllipses other)
        {
            return ListEquals(this.Rectangles, other.Rectangles)
                && this.Fill.Equals(other.Fill)
                && this.Stroke.Equals(other.Stroke)
                && this.Thickness == other.Thickness;
        }

        public override bool Transposed(DrawEllipses other)
        {
            return Transposed(this.Rectangles, other.Rectangles)
                && this.Fill.Equals(other.Fill)
                && this.Stroke.Equals(other.Stroke)
                && this.Thickness == other.Thickness;
        }
    }
}
