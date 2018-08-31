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

        public override bool Transposed(DrawRectangle other)
        {
            return Transposed(Rect, other.Rect)
                && Equals(Fill, other.Fill)
                && Equals(Stroke, other.Stroke)
                && Equals(Thickness, other.Thickness);
        }
    }
}
