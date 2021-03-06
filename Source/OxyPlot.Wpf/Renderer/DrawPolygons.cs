﻿using System.Collections.Generic;

namespace OxyPlot.Wpf
{
    internal class DrawPolygons : ADrawOperation<DrawPolygons>
    {
        public DrawPolygons(IList<IList<ScreenPoint>> polygons,
            OxyColor fill,
            OxyColor stroke,
            double thickness,
            double[] dashArray,
            LineJoin lineJoin,
            bool aliased)
        {
            Polygons = polygons;
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
        public IList<IList<ScreenPoint>> Polygons { get; }
        public OxyColor Stroke { get; }
        public double Thickness { get; }

        public override bool Equals(DrawPolygons other)
        {
            return ListEquals(Polygons, other.Polygons)
                && Fill.Equals(other.Fill)
                && Stroke.Equals(other.Stroke)
                && Thickness == other.Thickness
                && ArrayEquals(DashArray, other.DashArray)
                && LineJoin == other.LineJoin
                && Aliased == other.Aliased;
        }

        public override bool Transposed(DrawPolygons other)
        {
            return Transposed(Polygons, other.Polygons)
                && Fill.Equals(other.Fill)
                && Stroke.Equals(other.Stroke)
                && Thickness == other.Thickness
                && ArrayEquals(DashArray, other.DashArray)
                && LineJoin == other.LineJoin
                && Aliased == other.Aliased;
        }
    }
}