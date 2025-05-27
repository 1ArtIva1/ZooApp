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
        private void ReportsButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (TabItem tab in MainTabControl.Items)
            {
                if ((string)tab.Header == "Отчеты")
                {
                    tab.IsSelected = true;
                    return;
                }
            }

            var tabItem = new TabItem { Header = "Отчеты" };
            var frame = new Frame
            {
                Content = new Reports(),
                NavigationUIVisibility = NavigationUIVisibility.Hidden
            };
            tabItem.Content = frame;
            MainTabControl.Items.Add(tabItem);
            tabItem.IsSelected = true;
        }
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, есть ли уже вкладка "Главная"
            foreach (TabItem tab in MainTabControl.Items)
            {
                if ((string)tab.Header == "Главная")
                {
                    tab.IsSelected = true;
                    return;
                }
            }

            // Если вкладки нет — создаём новую
            var homeTab = new TabItem { Header = "Главная" };
            var homeFrame = new Frame
            {
                Source = new Uri("Pages/Homes.xaml", UriKind.Relative),
                NavigationUIVisibility = NavigationUIVisibility.Hidden
            };

            homeTab.Content = homeFrame;
            MainTabControl.Items.Add(homeTab);
            homeTab.IsSelected = true;
        }
        private void Storage_Click(object sender, RoutedEventArgs e)
        {
            foreach (TabItem tab in MainTabControl.Items)
            {
                if ((string)tab.Header == "Склад")
                {
                    tab.IsSelected = true;
                    return;
                }
            }

            var tabItem = new TabItem { Header = "Склад" };
            var frame = new Frame
            {
                Source = new Uri("Pages/Storage.xaml", UriKind.Relative),
                NavigationUIVisibility = NavigationUIVisibility.Hidden
            };
            tabItem.Content = frame;
            MainTabControl.Items.Add(tabItem);
            tabItem.IsSelected = true;
        }
        private void OperationsButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (TabItem tab in MainTabControl.Items)
            {
                if ((string)tab.Header == "Операции")
                {
                    tab.IsSelected = true;
                    return;
                }
            }

            var tabItem = new TabItem { Header = "Операции" };
            var frame = new Frame
            {
                Source = new Uri("Pages/Operations.xaml", UriKind.Relative),
                NavigationUIVisibility = NavigationUIVisibility.Hidden
            };
            tabItem.Content = frame;
            MainTabControl.Items.Add(tabItem);
            tabItem.IsSelected = true;


        }
    }
}
