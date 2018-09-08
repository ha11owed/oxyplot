namespace RefreshAnnotationsDemo
{
    using OxyPlot;
    using OxyPlot.Annotations;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    using System;
    using System.Linq;
    using System.Windows.Threading;

    internal class MainViewModel : IDisposable
    {
        private readonly Random rand = new Random();

        private double direction = 1;
        private double sweeplineX = 500;

        private DispatcherTimer timer = new DispatcherTimer();

        public MainViewModel()
        {
            Model = new PlotModel();
            Model.Title = "Generic Plot";

            // X Axis
            {
                LinearAxis axis = CreateLinearAxis();
                axis.Position = AxisPosition.Bottom;
                axis.Title = "RPM";
                Model.Axes.Add(axis);
            }

            // Y Axis
            {
                LinearAxis axis = CreateLinearAxis();
                axis.Position = AxisPosition.Left;
                axis.Title = "Performance";
                Model.Axes.Add(axis);
            }

            // Series
            SetSeries("Points", "RPM", 20);

            // Start a timer
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Start();
        }

        public PlotModel Model { get; }

        public void Dispose()
        {
            if (timer != null)
            {
                timer.Stop();
                timer = null;
            }
        }

        public void SetSeries(string tag, string title, int pointsCount)
        {
            LineSeries series = (LineSeries)Model.Series.FirstOrDefault(s => object.Equals(s.Tag, tag));
            if (series == null)
            {
                series = new LineSeries();
                series.Tag = tag;
                Model.Series.Add(series);
            }

            series.Title = title;
            series.MarkerType = MarkerType.Circle;
            series.MarkerFill = OxyColors.DarkBlue;
            series.Color = OxyColors.Blue;

            // Recreate the series
            series.Points.Clear();

            double x = 500;
            double y = 0;

            for (int i = 0; i < pointsCount; i++)
            {
                series.Points.Add(new DataPoint(x, y));
                x += 100 + rand.Next(0, 400);
                y = rand.Next(-100, 100);
            }
        }

        public void SetXSweepline(string tag, double x, string text, OxyColor color)
        {
            if (string.IsNullOrEmpty(tag)) { throw new ArgumentNullException(nameof(tag)); }

            LineAnnotation xSweepLine = (LineAnnotation)Model.Annotations.FirstOrDefault(la => object.Equals(la.Tag, tag));
            if (xSweepLine == null)
            {
                xSweepLine = new LineAnnotation();
                xSweepLine.Type = LineAnnotationType.Vertical;
                xSweepLine.Tag = tag;
                xSweepLine.StrokeThickness = 3;
                xSweepLine.Color = color;

                Model.Annotations.Add(xSweepLine);
            }
            xSweepLine.Text = text;
            xSweepLine.X = x;
        }

        private static LinearAxis CreateLinearAxis()
        {
            LinearAxis axis = new LinearAxis();
            axis.MajorGridlineStyle = LineStyle.Solid;
            axis.MinorGridlineStyle = LineStyle.Dot;
            return axis;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (sweeplineX > 3000)
            {
                direction = -1;
            }
            else if (sweeplineX < 500)
            {
                direction = 1;
            }
            sweeplineX += rand.Next(1, 5) * direction;

            // Sweepline
            SetXSweepline("Sweepline", sweeplineX, string.Format("RPM: {0:F2}", sweeplineX), OxyColors.Orange);

            // Invalidate
            Model.InvalidatePlot(false);
        }
    }
}
