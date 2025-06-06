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
using Home.Service;
using Home.Services;

namespace Home.Pages
{
    /// <summary>
    /// Логика взаимодействия для Operations.xaml
    /// </summary>
    public partial class Operations : Page
    {
        private List<ReceiptViewModel> _allReceipts = new List<ReceiptViewModel>();

        public Operations()
        {
            InitializeComponent();
            this.IsVisibleChanged += Operations_IsVisibleChanged;
            LoadReceipts();
        }

        private void Operations_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
            {
                LoadReceipts();
            }
        }

        private void LoadReceipts()
        {
            using (var db = new DatabaseService())
            {
                db.InitializeConnection(Databank.username, Databank.password);
                var receipts = db.LoadReceipts(); // Реализуйте этот метод для получения чеков из БД
                var users = db.LoadUsers();

                _allReceipts = receipts.Select(r => new ReceiptViewModel
                {
                    Id = r.Id,
                    Date = r.Date,
                    Seller = users.FirstOrDefault(u => u.Id == r.EmployeeId)?.Name ?? "—",
                    Total = r.Total
                }).OrderByDescending(r => r.Date).ToList();
            }
            ReceiptsDataGrid.ItemsSource = _allReceipts;
        }

        private void GroupComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GroupComboBox.SelectedIndex == 1) // По дням
                ReceiptsDataGrid.ItemsSource = _allReceipts.OrderByDescending(r => r.Date.Date).ToList();
            else if (GroupComboBox.SelectedIndex == 2) // По продавцам
                ReceiptsDataGrid.ItemsSource = _allReceipts.OrderBy(r => r.Seller).ToList();
            else
                ReceiptsDataGrid.ItemsSource = _allReceipts;
        }

        private void ReturnReceiptButton_Click(object sender, RoutedEventArgs e)
        {
            if (ReceiptsDataGrid.SelectedItem is ReceiptViewModel selected)
            {
                var result = MessageBox.Show(
                    $"Вы действительно хотите выполнить возврат и удалить чек №{selected.Id}?",
                    "Подтверждение возврата",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    using (var db = new DatabaseService())
                    {
                        db.InitializeConnection(Databank.username, Databank.password);
                        db.ReturnReceipt(selected.Id); // Возврат товара на склад
                        db.DeleteReceipt(selected.Id); // Удаление чека из БД
                    }
                    MessageBox.Show("Чек возвращён и удалён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadReceipts(); // обновляем таблицу чеков
                }
            }
            else
            {
                MessageBox.Show("Выберите чек для возврата.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ReceiptsDataGrid_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var row = ItemsControl.ContainerFromElement(ReceiptsDataGrid, e.OriginalSource as DependencyObject) as DataGridRow;
            if (row != null)
                ReceiptsDataGrid.SelectedItem = row.Item;
        }

        private void DetailsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (ReceiptsDataGrid.SelectedItem is ReceiptViewModel selected)
            {
                // Открыть окно с деталями чека
                var detailsWindow = new ReceiptDetailsWindow(selected.Id);
                detailsWindow.Owner = Window.GetWindow(this);
                detailsWindow.ShowDialog();
            }
        }
    }

    public class ReceiptViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Seller { get; set; }
        public decimal Total { get; set; }
    }
}
