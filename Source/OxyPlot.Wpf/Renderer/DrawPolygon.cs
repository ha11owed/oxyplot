﻿using System.Collections.Generic;

namespace OxyPlot.Wpf
{
    internal class DrawPolygon : ADrawOperation<DrawPolygon>
    {
        public DrawPolygon(IList<ScreenPoint> points,
            OxyColor fill, OxyColor stroke,
            double thickness,
            double[] dashArray,
            LineJoin lineJoin,
            bool aliased)
        {
            Points = points;
            Fill = fill;
            Stroke = stroke;
            Thickness = thickness;
            DashArray = dashArray;
            LineJoin = lineJoin;
            Aliased = aliased;
        }

        public bool Aliased { get; }
        public double[] DashArray { get; }
        public OxyColor Fill { get; }
        public LineJoin LineJoin { get; }
        public IList<ScreenPoint> Points { get; }
        public OxyColor Stroke { get; }
        public double Thickness { get; }

        public override bool Transposed(DrawPolygon other)
        {
            return Transposed(Points, other.Points)
                && Equals(Fill, other.Fill)
                && Equals(Stroke, other.Stroke)
                && Equals(Thickness, other.Thickness)
                && ArrayEquals(DashArray, other.DashArray)
                && LineJoin == other.LineJoin
                && Aliased == other.Aliased;
        }
    }
}
