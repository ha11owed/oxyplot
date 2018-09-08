namespace OxyPlot.Wpf
{
    internal class DrawEllipse : ADrawOperation<DrawEllipse>
    {
        public DrawEllipse(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
        {
            this.Rect = rect;
            this.Fill = fill;
            this.Stroke = stroke;
            this.Thickness = thickness;
        }

        public OxyColor Fill { get; }

        public OxyRect Rect { get; }

        public OxyColor Stroke { get; }

        public double Thickness { get; }

        public override bool Equals(DrawEllipse other)
        {
            return this.Rect.Equals(other.Rect)
                && this.Fill.Equals(other.Fill)
                && this.Stroke.Equals(other.Stroke)
                && this.Thickness == other.Thickness;
        }

        public override bool Transposed(DrawEllipse other)
        {
            return Transposed(this.Rect, other.Rect)
                && this.Fill.Equals(other.Fill)
                && this.Stroke.Equals(other.Stroke)
                && this.Thickness == other.Thickness;
        }
    }
}
