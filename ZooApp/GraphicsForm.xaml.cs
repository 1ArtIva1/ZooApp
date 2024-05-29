using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
using Npgsql;


namespace ZooApp
{
    /// <summary>
    /// Логика взаимодействия для GraphicsForm.xaml
    /// </summary>
    public partial class GraphicsForm : Window
    {
        public ObservableCollection<Fruit> Items4 { get; set; }
        //public SeriesCollection SeriesCollection { get; set; }
        public List<string> ProductLabels { get; set; }


        public GraphicsForm()
        {
            InitializeComponent();

            Time time = new Time();
            time.Timer_Day(label1);
            time.Timer_Clock(label2);
            time.Timer_Data(label3);

            DataContext = this;

            Items4 = new ObservableCollection<Fruit>();
            SeriesCollection = new SeriesCollection();


            List<Fruit> dataList = new List<Fruit>();


            NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; User Id=" + Databases.username + "; Password=" + Databases.password + "; Database=zoo");
            conn.Open();

            string query = "SELECT * FROM get_top_selling_products(10);";

            //NpgsqlCommand command = new NpgsqlCommand(query, conn);

            using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var fruit = new Fruit
                    {
                        Name = reader.GetString(1),
                        Qty = reader.GetInt32(2)
                    };
                    Items4.Add(fruit);
                }
            }

            foreach (var item in Items4) 
            {
                SeriesCollection.Add(new PieSeries
                {
                    Title = item.Name,
                    Values = new ChartValues<ObservableValue> { new ObservableValue(item.Qty) },
                    DataLabels = true
                });
            }

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

        private void MarginSaleGraph(object sender, RoutedEventArgs e)
        {
            string connString = "Server=localhost; User Id=" + Databases.username + "; Password=" + Databases.password + "; Database=zoo";

            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                string query = "SELECT * FROM calculate_price_difference_sale();";

                using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
                using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    var priceValues = new ChartValues<double>();
                    var marginValues = new ChartValues<double>();
                    var labels = new List<string>();

                    foreach (DataRow row in dataTable.Rows)
                    {
                        labels.Add(row["Название"].ToString());
                        marginValues.Add(Convert.ToDouble(row["Маржа"]));
                        priceValues.Add(Convert.ToDouble(row["Цена"]));
                    }

                    SeriesCollection2 = new SeriesCollection
                    {
                        new ColumnSeries
                        {
                            Title = "Продажа",
                            Values = priceValues
                        },
                        new ColumnSeries
                        {
                            Title = "Маржа",
                            Values = marginValues
                        }
                    };

                    ProductLabels = labels;

                    Graphic1.Series = SeriesCollection2;
                    Graphic1.AxisX[0].Labels = ProductLabels;
                }

                Graphic1.Visibility = Visibility.Visible;
                myPieChart.Visibility = Visibility.Hidden;
                Graphic2.Visibility = Visibility.Hidden;
            }
        }

        private void SaleExpensesGraph(object sender, RoutedEventArgs e)
        {
            string connString = "Server=localhost; User Id=" + Databases.username + "; Password=" + Databases.password + "; Database=zoo";

            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                string query = "SELECT * FROM calculate_price_difference_wholesale();";

                using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
                using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    var wholesalePriceValues = new ChartValues<double>();
                    var marginValues = new ChartValues<double>();
                    var labels = new List<string>();

                    foreach (DataRow row in dataTable.Rows)
                    {
                        labels.Add(row["Название"].ToString());
                        marginValues.Add(Convert.ToDouble(row["Маржа"]));
                        wholesalePriceValues.Add(Convert.ToDouble(row["Опт_Цена"]));
                    }

                    SeriesCollection2 = new SeriesCollection
                    {
                        new ColumnSeries
                        {
                            Title = "Опт. Цена",
                            Values = wholesalePriceValues
                        },
                        new ColumnSeries
                        {
                            Title = "Маржа",
                            Values = marginValues
                        }
                    };

                    ProductLabels = labels;

                    Graphic2.Series = SeriesCollection2;
                    Graphic2.AxisX[0].Labels = ProductLabels;
                }

                Graphic2.Visibility = Visibility.Visible;
                myPieChart.Visibility = Visibility.Hidden;
                Graphic1.Visibility = Visibility.Hidden;
            }
        }
    }
    
}
