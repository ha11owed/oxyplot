using System.Collections.Generic;

namespace OxyPlot.Wpf
{
    internal class DrawEllipses : ADrawOperation<DrawEllipses>
    {
        public DrawEllipses(IList<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness)
        {
            Rectangles = rectangles;
            Fill = fill;
            Stroke = stroke;
            Thickness = thickness;
        }

        public OxyColor Fill { get; }
        public IList<OxyRect> Rectangles { get; }
        public OxyColor Stroke { get; }
        public double Thickness { get; }

        public override bool Transposed(DrawEllipses other)
        {
            return Transposed(Rectangles, other.Rectangles)
                && Equals(Fill, other.Fill)
                && Equals(Stroke, other.Stroke)
                && Equals(Thickness, other.Thickness);
        }
    }
}
