using Home.Service;
using Home.Services;
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
    /// Логика взаимодействия для Sales.xaml
    /// </summary>
    public partial class Sales : Page
    {
        public Sales()
        {
            InitializeComponent();

            var products = new List<Product_Sale>
            {
           

            };


            SaleGrid.ItemsSource = products;
        }
        public class Product_Sale
        {
            public int Number { get; set; }
            public string Name { get; set; }
            public string Category { get; set; }
            public int Quantity { get; set; }
            public string Unit { get; set; }
            public decimal Price { get; set; }
            public decimal Sum { get; set; }
        }

        private void BarcodeButton_Click(object sender, RoutedEventArgs e)
        {
            var selector = new ProductSelectorWindow();
            selector.Owner = Window.GetWindow(this); // для привязки к основному окну

            // Показываем окно и проверяем, выбрал ли пользователь товар
            if (selector.ShowDialog() == true)
            {
                var selectedProduct = selector.SelectedProduct; 
                int selectedQty = selector.SelectedQuantity;
                if (selectedProduct != null)
                {
                    // Преобразуем в Product_Sale и добавим в таблицу
                    var currentList = (List<Product_Sale>)SaleGrid.ItemsSource;
                    int nextNumber = currentList.Count + 1;

                    currentList.Add(new Product_Sale
                    {
                        Number = nextNumber,
                        Name = selectedProduct.Name,
                        Category = selectedProduct.Category,
                        Quantity = selectedQty,
                        Unit = selectedProduct.Unit,
                        Price = selectedProduct.Price,
                        Sum = selectedProduct.Price * selectedQty
                    });

                    SaleGrid.Items.Refresh(); // обновляем таблицу
                }
            }
            UpdateTotalSum();
        }

        private void PaymentTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateChange();
        }

        private void UpdateChange()
        {
            decimal total = 0;
            decimal payment = 0;
            decimal change = 0;

            // Считываем значения из полей
            decimal.TryParse(TotalSumTextBlock.Text, out total);
            decimal.TryParse(PaymentTextBox.Text, out payment);

            if (payment >= total)
                change = payment - total;
            else
                change = 0;

            ChangeTextBlock.Text = change.ToString("0.00");
        }


        private void UpdateTotalSum()
        {
            var currentList = (List<Product_Sale>)SaleGrid.ItemsSource;
            decimal total = currentList.Sum(p => p.Sum);
            TotalSumTextBlock.Text = $"{total}";
            PaymentTextBox_TextChanged(null, null);
            UpdateChange();// обновить сдачу при изменении суммы
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            var currentList = (List<Product_Sale>)SaleGrid.ItemsSource;
            currentList.Clear();
            SaleGrid.Items.Refresh();
            UpdateTotalSum();
        }
        
        private void DeleteRowButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var row = button?.DataContext as Product_Sale;
            if (row == null) return;

            var currentList = (List<Product_Sale>)SaleGrid.ItemsSource;
            currentList.Remove(row);

            // Перенумеровать строки
            for (int i = 0; i < currentList.Count; i++)
                currentList[i].Number = i + 1;

            SaleGrid.Items.Refresh(); //try catch
            UpdateTotalSum();
        }

        
        private void CashButton_Click(object sender, RoutedEventArgs e)
        {

            var products = (List<Product_Sale>)SaleGrid.ItemsSource;
            if (products == null || products.Count == 0)
            {
                MessageBox.Show("Нет товаров для продажи.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            decimal total = products.Sum(p => p.Sum);

            // Получаем текущего пользователя (кассира)
            int employeeId = Databank.CurrentUserId; // предполагается, что у вас есть такой идентификатор

            var receipt = new Receipt
            {
                EmployeeId = employeeId,
                Date = DateTime.Now,
                Total = total
            };

            using (var db = new DatabaseService())
            {
                db.InitializeConnection(Databank.username, Databank.password);

                int receiptId = db.AddReceipt(receipt);
                var storageItems = db.LoadStorageItems();

                foreach (var p in products)
                {
                    var storageItem = storageItems.FirstOrDefault(s => s.Name == p.Name && s.Unit == p.Unit);
                    if (storageItem == null)
                        continue;

                    var item = new ReceiptItem
                    {
                        ReceiptId = receiptId,
                        StorageId = storageItem.Id,
                        DiscountId = null,
                        Quantity = p.Quantity,
                        Price = p.Price
                    };

                    db.AddReceiptItem(item);

                    // Уменьшаем количество на складе
                    int newQuantity = storageItem.Quantity - p.Quantity;
                    db.UpdateStorageItemQuantity(storageItem.Id, newQuantity);
                }
            }

            MessageBox.Show("Чек успешно сохранён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            // Очистить форму
            products.Clear();
            SaleGrid.Items.Refresh();
            UpdateTotalSum();
        }

        private void DeferButton_Click(object sender, RoutedEventArgs e)
        {
            var products = (List<Product_Sale>)SaleGrid.ItemsSource;
            var receipt = new DeferredReceipt { Products = products.ToList() };
            using (var db = new DatabaseService())
            {
                db.InitializeConnection(Databank.username, Databank.password);
                db.AddDeferredReceipt(receipt, Databank.CurrentUserId);
            }
            products.Clear();
            SaleGrid.Items.Refresh();
            UpdateTotalSum();
        }

        private void OpenDeferredButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new DatabaseService())
            {
                db.InitializeConnection(Databank.username, Databank.password);
                var deferredList = db.LoadDeferredReceipts();
                var window = new SelectDeferredReceiptWindow(deferredList);
                window.Owner = Window.GetWindow(this);
                if (window.ShowDialog() == true && window.SelectedReceipt != null)
                {
                    var products = (List<Product_Sale>)SaleGrid.ItemsSource;
                    products.Clear();
                    products.AddRange(window.SelectedReceipt.Products);
                    SaleGrid.Items.Refresh();
                    UpdateTotalSum();
                }
            }
        }

        private void CancelDeferredButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new DatabaseService())
            {
                db.InitializeConnection(Databank.username, Databank.password);
                var deferredList = db.LoadDeferredReceipts();
                var window = new SelectDeferredReceiptWindow(deferredList);
                window.Owner = Window.GetWindow(this);
                if (window.ShowDialog() == true && window.SelectedReceipt != null)
                {
                    db.DeleteDeferredReceipt(window.SelectedReceipt.Id);
                    MessageBox.Show("Отложенный чек аннулирован.");
                }
            }
        }
    }
}

