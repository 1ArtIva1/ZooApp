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
                    Title = "Purina Proplan",
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
            
            
            SeriesCollection2 = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "vail1",
                    Values = new ChartValues<double> { 5, 10, 20, 30 }
                },
                new ColumnSeries
                {
                    Title = "vail2",
                    Values = new ChartValues<double> { 10, 15, 20, 25 }
                }
            };


            SeriesCollection3 = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "vail1",
                    Values = new ChartValues<double> { 15, 25, 20, 45 }
                },
                new ColumnSeries
                {
                    Title = "vail2",
                    Values = new ChartValues<double> { 10, 15, 20, 25 }
                }
            };



            DataContext = this;

            BarLabels = new[] { "values 1", "values 2", "values 3", "values 4" };
            Formatter = value => value.ToString("N");


        }

        public SeriesCollection SeriesCollection { get; set; }

        public SeriesCollection SeriesCollection2 { get; set; }

        public SeriesCollection SeriesCollection3 { get; set; }


        public string[] BarLabels { get; set; }

        public Func<double,string> Formatter { get; set; }



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

        private void PieViz_Click(object sender, RoutedEventArgs e)
        {
            myPieChart.Visibility = Visibility.Visible;
            Graphic1.Visibility = Visibility.Hidden;
            Graphic2.Visibility = Visibility.Hidden;
        }

        private void Graphic_2(object sender, RoutedEventArgs e)
        {
            Graphic2.Visibility = Visibility.Visible;
            myPieChart.Visibility= Visibility.Hidden;
            Graphic1.Visibility = Visibility.Hidden;
        }

        private void Graphic_1(object sender, RoutedEventArgs e)
        {
            Graphic1.Visibility= Visibility.Visible;
            myPieChart.Visibility = Visibility.Hidden;
            Graphic2.Visibility = Visibility.Hidden;
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
    
}
