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

        public override bool Equals(DrawEllipse other)
        {
            return Rect.Equals(other.Rect)
                && Fill.Equals(other.Fill)
                && Stroke.Equals(other.Stroke)
                && Thickness == other.Thickness;
        }

        public override bool Transposed(DrawEllipse other)
        {
            return Transposed(Rect, other.Rect);
        }
    }
}
