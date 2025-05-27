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
    /// Логика взаимодействия для Storage.xaml
    /// </summary>
    public partial class Storage : Page
    {
        public Storage()
        {
            InitializeComponent();

            using (var db = new DatabaseService())
            {
                db.InitializeConnection(Databank.username, Databank.password);
                Databank.StorageItems = db.LoadStorageItems();
            }
            myDataGrid.ItemsSource = Databank.StorageItems;
        }

     

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddProductWindow();
            if (window.ShowDialog() == true)
            {
                using (var db = new DatabaseService())
                {
                    db.InitializeConnection(Databank.username, Databank.password);
                    Databank.StorageItems = db.LoadStorageItems();
                }
                myDataGrid.ItemsSource = Databank.StorageItems;
            }
            window.Owner = Application.Current.MainWindow;
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

        public void ApplyFilters()
        {
            IEnumerable<StorageItem> filtered = Databank.StorageItems; 

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

