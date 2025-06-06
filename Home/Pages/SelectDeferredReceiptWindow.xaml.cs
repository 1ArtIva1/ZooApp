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
using Home.Service;

namespace Home.Pages
{
    /// <summary>
    /// Логика взаимодействия для SelectDeferredReceiptWindow.xaml
    /// </summary>
    public partial class SelectDeferredReceiptWindow : Window
    {
        public DeferredReceipt SelectedReceipt { get; private set; }

        public SelectDeferredReceiptWindow(List<DeferredReceipt> receipts)
        {
            InitializeComponent();
            ReceiptsGrid.ItemsSource = receipts;
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedReceipt = ReceiptsGrid.SelectedItem as DeferredReceipt;
            if (SelectedReceipt != null)
                DialogResult = true;
            else
                MessageBox.Show("Выберите чек для открытия.");
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
