using System.Collections.Generic;
using System.Linq;

namespace OxyPlot.Wpf
{
    internal class DrawLine : ADrawOperation<DrawLine>
    {
        public DrawLine(IList<ScreenPoint> points, OxyColor stroke, double thickness, double[] dashArray, LineJoin lineJoin, bool aliased, DrawLineType type)
        {
            this.Points = (points == null) ? null : points.ToList();
            this.Stroke = stroke;
            this.Thickness = thickness;
            this.DashArray = dashArray;
            this.LineJoin = lineJoin;
            this.Aliased = aliased;
            this.Type = type;
        }

        public bool Aliased { get; }

        public double[] DashArray { get; }

        public LineJoin LineJoin { get; }

        public IList<ScreenPoint> Points { get; }

        public OxyColor Stroke { get; }

        public double Thickness { get; }

        public DrawLineType Type { get; }

        public override bool Equals(DrawLine other)
        {
            return ListEquals(this.Points, other.Points)
                && this.Aliased == other.Aliased
                && ArrayEquals(this.DashArray, other.DashArray)
                && this.LineJoin == other.LineJoin
                && this.Stroke == other.Stroke
                && this.Thickness == other.Thickness
                && this.Type == other.Type;
        }

        public override bool Transposed(DrawLine other)
        {
            return Transposed(this.Points, other.Points)
                && this.Aliased == other.Aliased
                && ArrayEquals(this.DashArray, other.DashArray)
                && this.LineJoin == other.LineJoin
                && this.Stroke == other.Stroke
                && this.Thickness == other.Thickness
                && this.Type == other.Type;
        }
    }
}
