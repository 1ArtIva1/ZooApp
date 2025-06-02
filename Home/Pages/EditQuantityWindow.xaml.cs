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
    /// Логика взаимодействия для EditQuantityWindow.xaml
    /// </summary>
    public partial class EditQuantityWindow : Window
    {
        public int? NewQuantity { get; private set; }

        public EditQuantityWindow(string productName, int currentQuantity)
        {
            InitializeComponent();
            ProductNameTextBlock.Text = $"Товар: {productName}";
            QuantityTextBox.Text = currentQuantity.ToString();
            QuantityTextBox.Focus();
            QuantityTextBox.SelectAll();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(QuantityTextBox.Text, out int qty) && qty >= 0)
            {
                NewQuantity = qty;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Введите корректное неотрицательное число.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
