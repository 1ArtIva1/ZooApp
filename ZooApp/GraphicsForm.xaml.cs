using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Definitions.Charts;
using LiveCharts.Wpf;

namespace ZooApp
{
    /// <summary>
    /// Логика взаимодействия для GraphicsForm.xaml
    /// </summary>
    public partial class GraphicsForm : Window
    {
        public GraphicsForm()
        {
            InitializeComponent();

            Time time = new Time();
            time.Timer_Day(label1);
            time.Timer_Clock(label2);
            time.Timer_Data(label3);

            SeriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "t1",
                    Values = new ChartValues<ObservableValue> {new ObservableValue(10) },
                    DataLabels = true
                },
                 new PieSeries
                {
                    Title = "t2",
                    Values = new ChartValues<ObservableValue> {new ObservableValue(20) },
                    DataLabels = true
                },
                  new PieSeries
                {
                    Title = "t3",
                    Values = new ChartValues<ObservableValue> {new ObservableValue(33) },
                    DataLabels = true
                },
                   new PieSeries
                {
                    Title = "t4 ",
                    Values = new ChartValues<ObservableValue> {new ObservableValue(24) },
                    DataLabels = true
                },
            };
            DataContext = this;

        }

        public SeriesCollection SeriesCollection { get; set; }

        




        private void Back_Click(object sender, RoutedEventArgs e)
        {
                ReportingForm reportingForm = new ReportingForm();
                reportingForm.Show();
                this.Close();
        }

        private void myPieChart_DataClick(object sender, ChartPoint chartPoint)
        {
            MessageBox.Show("Текущее значение: " + chartPoint.Y + "(" + (chartPoint.Participation * 100).ToString() + "%");
        }
    }
    
}
