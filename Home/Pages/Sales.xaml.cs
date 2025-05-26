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
                        Quantity = 1,
                        Unit = selectedProduct.Unit,
                        Price = selectedProduct.Price,
                        Sum = (selectedProduct.Price) // пока без умножения, если 1 шт.
                    });

                    SaleGrid.Items.Refresh(); // обновляем таблицу
                }
            }
            UpdateTotalSum();
        }
        private void UpdateTotalSum()
        {
            var currentList = (List<Product_Sale>)SaleGrid.ItemsSource;
            decimal total = currentList.Sum(p => p.Sum);
            TotalSumTextBlock.Text = $"{total}";
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

            SaleGrid.Items.Refresh();
            UpdateTotalSum();
        }
    }
}

