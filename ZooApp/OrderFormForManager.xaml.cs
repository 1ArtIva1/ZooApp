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
    
    public partial class OrderFormForManager : Window
    {
        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; User Id= " + Databases.username + "; Password=" + Databases.password + "; Database = zoo");

        public OrderFormForManager()
        {

           


            InitializeComponent();

            Time time = new Time();
            time.Timer_Day(label11);
            time.Timer_Clock(label2);
            time.Timer_Data(label3);

        }

    

        

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (Databases.username == "postgres")
            {
                MainManager mainManager = new MainManager();
                mainManager.Show();
                this.Close();
            }
            else
            {
                MainSeller mainSeller = new MainSeller();
                mainSeller.Show();
                this.Close();
            }
        }

        private void DataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
