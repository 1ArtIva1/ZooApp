using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для OrderForm.xaml
    /// </summary>
    public partial class OrderForm : Window
    {
        public OrderForm()
        {
            InitializeComponent();

            Time time = new Time();
            time.Timer_Day(label1);
            time.Timer_Clock(label2);
            time.Timer_Data(label3);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainSeller mainSeller = new MainSeller();
            mainSeller.Show();
            this.Close();
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
