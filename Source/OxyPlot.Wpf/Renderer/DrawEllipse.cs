namespace OxyPlot.Wpf
{
    internal class DrawEllipse : ADrawOperation<DrawEllipse>
    {
        public DrawEllipse(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
        {
            Rect = rect;
            Fill = fill;
            Stroke = stroke;
            Thickness = thickness;
        }

        public OxyColor Fill { get; }
        public OxyRect Rect { get; }
        public OxyColor Stroke { get; }
        public double Thickness { get; }

        public override bool Transposed(DrawEllipse other)
        {
            return Transposed(Rect, other.Rect)
                && Equals(Fill, other.Fill)
                && Equals(Stroke, other.Stroke)
                && Equals(Thickness, other.Thickness);
        }
    }
}
