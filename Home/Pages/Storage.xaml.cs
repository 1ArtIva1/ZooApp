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
    /// Логика взаимодействия для Storage.xaml
    /// </summary>
    public partial class Storage : Page
    {
        private List<Product> allProducts = new List<Product>();
        public Storage()
        {
            InitializeComponent();

            // Исходный список продуктов
            allProducts = new List<Product>
            {
                new Product { Name = "Игрушка для собак", Quantity = 15, Category = "Аксессуары", Price = 350 },
                new Product { Name = "Игрушка для кошек", Quantity = 10, Category = "Аксессуары", Price = 350 },
                new Product { Name = "Игрушка для хомячков", Quantity = 0, Category = "Аксессуары", Price = 350 },
                new Product { Name = "Корм для собак", Quantity = 20, Category = "Корма", Price = 1000 },
                new Product { Name = "Корм для кошек", Quantity = 0, Category = "Корма", Price = 120 },
                new Product { Name = "Наполнитель", Quantity = 8, Category = "Гигиена", Price = 800 },
            };

            myDataGrid.ItemsSource = allProducts;
        }

        public class Product
        {
            public string Name { get; set; }
            public int Quantity { get; set; }
            public string Category { get; set; }
            public decimal Price { get; set; }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddProductWindow();
            window.Owner = Application.Current.MainWindow;
            window.ShowDialog();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ViewComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            IEnumerable<Product> filtered = allProducts; 

            // Поиск
            string searchText = SearchBox.Text?.ToLower();
            if (!string.IsNullOrEmpty(searchText))
            {
                filtered = filtered.Where(p => p.Name.ToLower().Contains(searchText));
            }

            // Фильтрация по наличию
            var viewItem = (ViewComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            switch (viewItem)
            {
                case "Товары на складе":
                    filtered = filtered.Where(p => p.Quantity > 0);
                    break;
                case "Недоступные товары":
                    filtered = filtered.Where(p => p.Quantity == 0);
                    break;
            }

            // Сортировка
            var sortItem = (SortComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            switch (sortItem)
            {
                case "По категориям":
                    filtered = filtered.OrderBy(p => p.Category);
                    break;
                case "По алфавиту":
                    filtered = filtered.OrderBy(p => p.Name);
                    break;
                case "По количеству":
                    filtered = filtered.OrderByDescending(p => p.Quantity);
                    break;
            }

            myDataGrid.ItemsSource = filtered.ToList();
        }
    }


}

