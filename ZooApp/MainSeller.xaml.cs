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
    /// Логика взаимодействия для MainSeller.xaml
    /// </summary>
    public partial class MainSeller : Window
    {
        public MainSeller()
        {
            InitializeComponent();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void ArrangeSellForm_Click(object sender, RoutedEventArgs e)
        {
            ArrangeSellForm arrangeSellForm = new ArrangeSellForm();
            arrangeSellForm.Show();
            this.Close();
        }

        private void OrderForm_Click(object sender, RoutedEventArgs e)
        {
            OrderForm orderForm = new OrderForm();
            orderForm.Show();
            this.Close();
        }

        

        private void OrderForManager_Click_1(object sender, RoutedEventArgs e)
        {
            OrderFormForManager orderFormForManager = new OrderFormForManager();
            orderFormForManager.Show();
            this.Close();
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Close();
        }
    }
}
