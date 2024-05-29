using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

namespace ZooApp
{
    /// <summary>
    /// Логика взаимодействия для ReportForm.xaml
    /// </summary>
    public partial class ReportForm : Window
    {
        public ReportForm()
        {
            
            InitializeComponent();
    
            Time time = new Time();
            time.Timer_Day(label1);
            time.Timer_Clock(label2);
            time.Timer_Data(label3);

        }

        private void SalesData(object sender, RoutedEventArgs e)
        {
            DataGrid3.ItemsSource = null;

            // Подключение к базе данных PostgreSQL
            string connString = "Server=localhost; User Id=" + Databases.username + "; Password=" + Databases.password + "; Database=zoo";

            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                // Запрос для вызова функции
                string query = "SELECT * FROM get_selling_();";

                using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
                using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Привязка данных к DataGrid
                    DataGrid3.ItemsSource = dataTable.DefaultView;
                }
            }
        }

        private void MarginData(object sender, RoutedEventArgs e)
        {
            DataGrid3.ItemsSource = null;

            // Подключение к базе данных PostgreSQL
            string connString = "Server=localhost; User Id=" + Databases.username + "; Password=" + Databases.password + "; Database=zoo";

            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                // Запрос для вызова функции
                string query = "SELECT * FROM calculate_margin();";

                using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
                using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Привязка данных к DataGrid
                    DataGrid3.ItemsSource = dataTable.DefaultView;
                }
            }
        }

        private void ProductsData(object sender, RoutedEventArgs e)
        {
            DataGrid3.ItemsSource = null;

            // Подключение к базе данных PostgreSQL
            string connString = "Server=localhost; User Id=" + Databases.username + "; Password=" + Databases.password + "; Database=zoo";

            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                // Запрос для вызова функции
                string query = "SELECT * FROM get_top_products();"; //SELECT * FROM get_name_products();

                using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
                using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Привязка данных к DataGrid
                    DataGrid3.ItemsSource = dataTable.DefaultView;
                }
            }
        }

        private void StorageData(object sender, RoutedEventArgs e)
        {
            DataGrid3.ItemsSource = null;

            // Подключение к базе данных PostgreSQL
            string connString = "Server=localhost; User Id=" + Databases.username + "; Password=" + Databases.password + "; Database=zoo";

            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                // Запрос для вызова функции
                string query = "SELECT * FROM get_name_products();"; 

                using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
                using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Привязка данных к DataGrid
                    DataGrid3.ItemsSource = dataTable.DefaultView;
                }
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ReportingForm reportingForm = new ReportingForm();
            reportingForm.Show();
            this.Close();
        }

    }
}
