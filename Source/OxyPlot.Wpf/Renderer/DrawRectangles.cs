using System.Collections.Generic;

namespace OxyPlot.Wpf
{
    internal class DrawRectangles : ADrawOperation<DrawRectangles>
    {
        public DrawRectangles(IList<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness)
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

        public override bool Equals(DrawRectangles other)
        {
            return ListEquals(Rectangles, other.Rectangles)
                && Fill.Equals(other.Fill)
                && Stroke.Equals(other.Stroke)
                && Thickness == other.Thickness;
        }

        public override bool Transposed(DrawRectangles other)
        {
            return Transposed(Rectangles, other.Rectangles)
                && Equals(Fill, other.Fill)
                && Equals(Stroke, other.Stroke)
                && Equals(Thickness, other.Thickness);
        }
    }
}
