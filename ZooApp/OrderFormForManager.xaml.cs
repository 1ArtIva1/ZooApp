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
    /// Логика взаимодействия для OrderFormForManager.xaml
    /// </summary>
    public partial class OrderFormForManager : Window
    {
        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; User Id= " + Databases.username + "; Password=" + Databases.password + "; Database = postgres");

        public OrderFormForManager()
        {
            InitializeComponent();

            conn.Open();
            NpgsqlCommand comm = new NpgsqlCommand();
            comm.Connection = conn;
            comm.CommandType = CommandType.Text;
            comm.CommandText = "select * from items";
            NpgsqlDataReader dr = comm.ExecuteReader();
            if (dr.HasRows)
            {
                DataTable dt = new DataTable();
                dt.Load(dr);
                DataGrid1.ItemsSource = dt.DefaultView;
                comm.Dispose();
                conn.Close();

            }

        }

    

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainManager mainManager = new MainManager();
            mainManager.Show();
            this.Close();
        }

        private void DataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
