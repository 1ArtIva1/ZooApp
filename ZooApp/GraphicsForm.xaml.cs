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
    /// Логика взаимодействия для GraphicsForm.xaml
    /// </summary>
    public partial class GraphicsForm : Window
    {
        public GraphicsForm()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ReportingForm reportingForm = new ReportingForm();
            reportingForm.Show();
            this.Close();
        }
    }
}
