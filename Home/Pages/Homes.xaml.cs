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
using System.Windows.Threading;
using Home.Service;
using Home.Services;

namespace Home.Pages
{
    /// <summary>
    /// Логика взаимодействия для Home.xaml
    /// </summary>
    public partial class Homes : Page
    {
        // Статическое поле для хранения состояния смены
        private static bool shiftStarted = false;
        private DispatcherTimer _clockTimer;
        private static DateTime? shiftStartTime = null;

        public Homes()
        {
            InitializeComponent();

            _clockTimer = new DispatcherTimer();
            _clockTimer.Interval = TimeSpan.FromSeconds(1);
            _clockTimer.Tick += ClockTimer_Tick;
            _clockTimer.Start();

            ClockTextBlock.Text = DateTime.Now.ToString("HH:mm");
            UpdateShiftStartTimeUI();
            UpdateWorkDurationUI();

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
            UpdateStatistics();
        }

        private void StartShiftButton_Click(object sender, RoutedEventArgs e)
        {
            shiftStarted = true;
            shiftStartTime = DateTime.Now;
            StartShiftButton.Visibility = Visibility.Collapsed;
            MainContentPanel.Visibility = Visibility.Visible;
            UpdateShiftStartTimeUI();
        }
        private void UpdateShiftStartTimeUI()
        {
            if (shiftStartTime.HasValue)
                ShiftStartTimeTextBlock.Text = shiftStartTime.Value.ToString("HH:mm");
            else
                ShiftStartTimeTextBlock.Text = "--:--";
        }
        private void UpdateWorkDurationUI()
        {
            if (shiftStartTime.HasValue)
            {
                TimeSpan duration = DateTime.Now - shiftStartTime.Value;
                WorkDurationTextBlock.Text = duration.ToString(@"hh\:mm");
            }
            else
            {
                WorkDurationTextBlock.Text = "00:00";
            }
        }

        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            ClockTextBlock.Text = DateTime.Now.ToString("HH:mm");
            UpdateWorkDurationUI();
        }

        private void UpdateStatistics()
        {
            using (var db = new DatabaseService())
            {
                db.InitializeConnection(Databank.username, Databank.password);
                var receipts = db.LoadReceipts();
                var today = DateTime.Today;
                var todayReceipts = receipts.Where(r => r.Date.Date == today).ToList();

                decimal totalSum = todayReceipts.Sum(r => r.Total);
                int count = todayReceipts.Count;
                decimal avg = count > 0 ? totalSum / count : 0;

                AvgReceiptTextBlock.Text = avg.ToString("0.00");
                TotalDayTextBlock.Text = totalSum.ToString("0.00");
                CountDayTextBlock.Text = count.ToString();
            }
        }
    }
}
