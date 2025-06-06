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
using Home.Services;

namespace Home.Pages
{
    /// <summary>
    /// Логика взаимодействия для ReceiptDetailsWindow.xaml
    /// </summary>
    public partial class ReceiptDetailsWindow : Window
    {
        public ReceiptDetailsWindow(int receiptId)
        {
            InitializeComponent();
            LoadReceiptItems(receiptId);
        }

        private void LoadReceiptItems(int receiptId)
        {
            using (var db = new DatabaseService())
            {
                db.InitializeConnection(Databank.username, Databank.password);
                var items = db.LoadReceiptItemsByReceiptId(receiptId); // Реализуйте этот метод
                DetailsDataGrid.ItemsSource = items;
            }
        }
    }
}
