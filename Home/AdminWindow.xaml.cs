using Home.Pages;
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

namespace Home
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
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

        private void FinanceButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, есть ли вкладка "Продажи"
            foreach (TabItem tab in MainTabControl.Items)
            {
                if ((string)tab.Header == "Финансы")
                {
                    tab.IsSelected = true;
                    return;
                }
            }

            var tabItem = new TabItem { Header = "Финансы" };
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

        private void AdminButton_Click(object sender, RoutedEventArgs e)
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
