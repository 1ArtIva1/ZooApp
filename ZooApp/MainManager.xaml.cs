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
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class MainManager : Window
    {
        public MainManager()
        {
            InitializeComponent();

            Time time = new Time();
            time.Timer_Day(label1);
            time.Timer_Clock(label2);
            time.Timer_Data(label3);
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            FirstRegForm regform1 = new FirstRegForm();
            regform1.Show();
            this.Close();

        }

        private void ChangeUser_Click(object sender, RoutedEventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Close();
        }

        private void Reporting_Click(object sender, RoutedEventArgs e)
        {
            ReportingForm reportingForm = new ReportingForm();
            reportingForm.Show();
            this.Close();
        }

        private void OrderForManager_Click(object sender, RoutedEventArgs e)
        {
            OrderFormForManager orderFormForManager = new OrderFormForManager();
            orderFormForManager.Show();
            this.Close();
        }
    }
}
