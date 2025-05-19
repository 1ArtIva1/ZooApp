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

        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            while (current != null)
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            return null;
        }

        private void DiscountsButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (TabItem tab in MainTabControl.Items)
            {
                if ((string)tab.Header == "Администрирование")
                {
                    tab.IsSelected = true;
                    return;
                }
            }

            var tabItem = new TabItem { Header = "Администрирование" };
            var frame = new Frame
            {
                Source = new Uri("Pages/Administration.xaml", UriKind.Relative),
                NavigationUIVisibility = NavigationUIVisibility.Hidden
            };
            tabItem.Content = frame;
            MainTabControl.Items.Add(tabItem);
            tabItem.IsSelected = true;
        }

        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
