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

namespace Home.Pages
{
    /// <summary>
    /// Логика взаимодействия для Home.xaml
    /// </summary>
    public partial class Homes : Page
    {
        // Статическое поле для хранения состояния смены
        private static bool shiftStarted = false;

        public Homes()
        {
            InitializeComponent();

            // Если смена уже начата, скрываем кнопку и показываем контент
            if (shiftStarted)
            {
                StartShiftButton.Visibility = Visibility.Collapsed;
                MainContentPanel.Visibility = Visibility.Visible;
            }
            else
            {
                StartShiftButton.Visibility = Visibility.Visible;
                MainContentPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void StartShiftButton_Click(object sender, RoutedEventArgs e)
        {
            shiftStarted = true;
            StartShiftButton.Visibility = Visibility.Collapsed;
            MainContentPanel.Visibility = Visibility.Visible;
        }
    }
}
