using System.Collections.Generic;

namespace OxyPlot.Wpf
{
    internal class DrawLine : ADrawOperation<DrawLine>
    {
        public DrawLine(IList<ScreenPoint> points, OxyColor stroke, double thickness, double[] dashArray, LineJoin lineJoin, bool aliased, DrawLineType type)
        {
            Points = points;
            Stroke = stroke;
            Thickness = thickness;
            DashArray = dashArray;
            LineJoin = lineJoin;
            Aliased = aliased;
        }

        public bool Aliased { get; }
        public double[] DashArray { get; }
        public LineJoin LineJoin { get; }
        public IList<ScreenPoint> Points { get; }
        public OxyColor Stroke { get; }
        public double Thickness { get; }
        public DrawLineType Type { get; }

        public override bool Transposed(DrawLine other)
        {
            return Aliased == other.Aliased
                && ArrayEquals(DashArray, other.DashArray)
                && LineJoin == other.LineJoin
                && Transposed(Points, other.Points)
                && Stroke == other.Stroke
                && Thickness == other.Thickness
                && Type == other.Type;
        }
    }
}
