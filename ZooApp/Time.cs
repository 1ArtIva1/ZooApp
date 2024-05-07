using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
    internal class Time
    {
        public void Timer_Day(System.Windows.Controls.Label label_day)
        {

            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.IsEnabled = true;
            timer.Tick += (o, t) => { label_day.Content = DateTime.Now.ToString("dddd"); };
            timer.Start();
        }
        public void Timer_Clock(System.Windows.Controls.Label label_clock)
        {

            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.IsEnabled = true;
            timer.Tick += (o, t) => { label_clock.Content = DateTime.Now.ToString("HH:mm"); };
            timer.Start();
        }

        public void Timer_Data(System.Windows.Controls.Label label_data)
        {

            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.IsEnabled = true;
            timer.Tick += (o, t) => { label_data.Content = DateTime.Now.ToString("dd/MM/yyyy"); };
            timer.Start();
            }
        }
}
