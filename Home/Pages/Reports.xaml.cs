using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;

namespace Home.Pages
{
    public partial class Reports : Page
    {
        public SeriesCollection PieSeries { get; set; }

        public Reports()
        {
            InitializeComponent();

            PieSeries = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Первое значение",
                    Values = new ChartValues<double> { 8 },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Второе значение",
                    Values = new ChartValues<double> { 6 },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Третее значение",
                    Values = new ChartValues<double> { 10 },
                    DataLabels = true
                }
            };

            DataContext = this; 
        }
    }
}
