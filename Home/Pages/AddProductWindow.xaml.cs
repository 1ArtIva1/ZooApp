using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Логика взаимодействия для AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        public AddProductWindow()
        {
            InitializeComponent();
        }
        private void UpdatePrice(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(PurchasePriceTextBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double purchasePrice) &&
                double.TryParse(MarkupTextBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double markup))
            {
                double sellingPrice = purchasePrice + (purchasePrice * markup / 100);
                SellingPriceTextBlock.Text = sellingPrice.ToString("F2");
            }
            else
            {
                SellingPriceTextBlock.Text = "0";
            }
        }
    }
}
