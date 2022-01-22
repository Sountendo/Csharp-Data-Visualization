namespace SplineInterpolationViewer
{
    public partial class Form1 : Form
    {
        readonly ScottPlot.Plottable.ScatterPlotDraggable Scatter1;
        readonly ScottPlot.Plottable.ScatterPlotList Scatter2;

        public Form1()
        {
            InitializeComponent();

            Random Rand = new(0);
            int pointCount = 20;
            double[] xs = ScottPlot.DataGen.RandomWalk(Rand, pointCount);
            double[] ys = ScottPlot.DataGen.RandomWalk(Rand, pointCount);

            Scatter1 = new(xs, ys);
            Scatter1.Color = formsPlot1.Plot.Palette.GetColor(0);
            Scatter1.MarkerSize = 7;
            Scatter1.DragEnabled = true;
            formsPlot1.Plot.Add(Scatter1);

            Scatter2 = new();
            Scatter2.Color = formsPlot1.Plot.Palette.GetColor(1);
            formsPlot1.Plot.Add(Scatter2);

            formsPlot1.PlottableDragged += FormsPlot1_PlottableDragged;

            formsPlot1.Configuration.Quality = ScottPlot.Control.QualityMode.High;

            ReInterpolate();
        }

        private void FormsPlot1_PlottableDragged(object? sender, EventArgs e) => ReInterpolate();

        private void trackBar1_Scroll(object sender, EventArgs e) => ReInterpolate();

        private void cbShowPoints_CheckedChanged(object sender, EventArgs e) => ReInterpolate();

        private void ReInterpolate()
        {
            lblSmoothness.Text = $"Smooth: {tbSmoothness.Value}";

            (double[] xs2, double[] ys2) = Interpolation.Cubic.InterpolateXY(Scatter1.Xs, Scatter1.Ys, tbSmoothness.Value);
            Scatter2.Clear();
            Scatter2.AddRange(xs2, ys2);
            Scatter2.MarkerSize = cbShowPoints.Checked ? 4 : 0;

            formsPlot1.Plot.AxisAuto();
            formsPlot1.Refresh(skipIfCurrentlyRendering: true);
        }
    }
}