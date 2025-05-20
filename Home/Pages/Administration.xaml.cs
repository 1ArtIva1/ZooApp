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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Home.Pages;

namespace Home.Pages
{
    /// <summary>
    /// Логика взаимодействия для Adminstration.xaml
    /// </summary>
    public partial class Adminstration : Page
    {
        public Adminstration()
        {
            InitializeComponent();
        }

        private void DiscountsButton_Click(object sender, RoutedEventArgs e)
        {
            
            var stackPanel = new StackPanel();
            var frame = new Frame
            {
                Source = new Uri("Pages/Discounts.xaml", UriKind.Relative),
                NavigationUIVisibility = NavigationUIVisibility.Hidden
            };
            stackPanel.Children.Add(frame);
        }

        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
