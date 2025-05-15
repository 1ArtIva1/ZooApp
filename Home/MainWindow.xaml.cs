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


namespace Home
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        public void CloseTab_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var tabItem = FindAncestor<TabItem>(button);
            if (tabItem == null) return;

            var tabControl = tabItem.Parent as TabControl;
            if (tabControl != null)
            {
                tabControl.Items.Remove(tabItem);
            }
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
        private void SalesButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, есть ли вкладка "Продажи"
            foreach (TabItem tab in MainTabControl.Items)
            {
                if ((string)tab.Header == "Продажи")
                {
                    tab.IsSelected = true;
                    return;
                }
            }

            var tabItem = new TabItem { Header = "Продажи" };
            var frame = new Frame
            {
                Source = new Uri("Pages/Sales.xaml", UriKind.Relative),
                NavigationUIVisibility = NavigationUIVisibility.Hidden
            };
            tabItem.Content = frame;
            MainTabControl.Items.Add(tabItem);
            tabItem.IsSelected = true;
        }

    }
}
