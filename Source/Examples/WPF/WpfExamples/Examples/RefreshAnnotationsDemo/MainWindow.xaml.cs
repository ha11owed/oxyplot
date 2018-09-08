namespace RefreshAnnotationsDemo
{
    using OxyPlot;

    using WpfExamples;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Demonstrates the RefreshAnnotationsDemo.")]
    public partial class MainWindow
    {
        private MainViewModel viewModel;

        public MainWindow()
        {
            viewModel = new MainViewModel();
            Closed += MainWindow_Closed;
            this.InitializeComponent();
        }

        public PlotModel PlotModel
        {
            get { return viewModel.Model; }
        }

        private void MainWindow_Closed(object sender, System.EventArgs e)
        {
            if (viewModel != null)
            {
                viewModel.Dispose();
                viewModel = null;
            }
        }
    }
}
