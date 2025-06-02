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
    /// Логика взаимодействия для Discounts.xaml
    /// </summary>
    public partial class Discounts : Page
    {
        public Discounts()
        {
            InitializeComponent();
            
            using (var db = new DatabaseService())
            {
                db.InitializeConnection(Databank.username, Databank.password);
                Databank.DiscountsList = db.LoadDiscounts();
            }
            myDataGrid.ItemsSource = Databank.DiscountsList;
        }

        private void ViewComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddDiscountWindow();
            if (window.ShowDialog() == true)
            {
                using (var db = new DatabaseService())
                {
                    db.InitializeConnection(Databank.username, Databank.password);
                    Databank.DiscountsList = db.LoadDiscounts();
                }
                myDataGrid.ItemsSource = Databank.DiscountsList;
            }
            window.Owner = Application.Current.MainWindow;
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = myDataGrid.SelectedItem as DiscountsList;
            if (selectedItem == null)
            {
                MessageBox.Show("Выберите скидку для удаления.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Вы действительно хотите удалить скидку \"{selectedItem.Name}\"?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                using (var db = new DatabaseService())
                {
                    db.InitializeConnection(Databank.username, Databank.password);
                    // есть метод для удаления товара по Id
                    db.DeleteDiscount(selectedItem.Id);
                }
                // Обновляем список
                Databank.DiscountsList.Remove(selectedItem);
                myDataGrid.ItemsSource = null;
                myDataGrid.ItemsSource = Databank.DiscountsList;
            }
        }
    }
}
