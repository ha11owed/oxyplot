namespace OxyPlot.Wpf
{
    internal class DrawRectangle : ADrawOperation<DrawRectangle>
    {
        public DrawRectangle(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
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

        public override bool Equals(DrawRectangle other)
        {
            return Rect.Equals(other.Rect)
                && Fill.Equals(other.Fill)
                && Stroke.Equals(other.Stroke)
                && Thickness == other.Thickness;
        }

        public override bool Transposed(DrawRectangle other)
        {
            return Transposed(Rect, other.Rect);
        }
    }
}
