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
using System.Windows.Shapes;

namespace Home.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductSelectorWindow.xaml
    /// </summary>
    public partial class ProductSelectorWindow : Window
    {
        private List<StorageItem> allProducts;
        public StorageItem SelectedProduct { get; private set; }
        public int SelectedQuantity { get; private set; } = 1;
        public ProductSelectorWindow()
        {
            InitializeComponent();
            LoadProducts();
        }
        private void LoadProducts()
        {
            using (var db = new DatabaseService())
            {
                db.InitializeConnection(Databank.username, Databank.password);
                allProducts = db.LoadStorageItems();
            }

            ApplyFilters();
        }
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (allProducts == null)
                return;
           

            IEnumerable<StorageItem> filtered = allProducts;

            // Поиск
            string searchText = SearchBox.Text?.ToLower();
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                filtered = filtered.Where(p => p.Name.ToLower().Contains(searchText));
            }

            // Сортировка
            var sortItem = (SortComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            switch (sortItem)
            {
                case "По алфавиту":
                    filtered = filtered.OrderBy(p => p.Name);
                    break;
                case "По категории":
                    filtered = filtered.OrderBy(p => p.Category);
                    break;
                case "По количеству":
                    filtered = filtered.OrderByDescending(p => p.Quantity);
                    break;
            }

            ProductDataGrid.ItemsSource = filtered.ToList();
        }
        private void ProductGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ProductDataGrid.SelectedItem is StorageItem selectedItem)
            {
                if (selectedItem.Quantity <= 0)
                {
                    MessageBox.Show("Нельзя добавить товар с нулевым остатком!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Открываем окно для выбора количества
                var editWindow = new EditQuantityWindow(selectedItem.Name, 1)
                {
                    Owner = this
                };
                if (editWindow.ShowDialog() == true && editWindow.NewQuantity.HasValue)
                {
                    int qty = editWindow.NewQuantity.Value;
                    if (qty > selectedItem.Quantity)
                    {
                        MessageBox.Show("Недостаточно товара на складе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    SelectedProduct = selectedItem;
                    SelectedQuantity = qty;
                    DialogResult = true;
                    Close();
                }
            }
        }
    }
}
