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
            new Product_Sale { Number = 1, Name = "Игрушка для кошек", Quantity = 2, Unit= "шт.", Price = 350, Sum = 700},
            new Product_Sale { Number = 2, Name = "Игрушка для хомячков", Quantity = 1, Unit= "шт.", Price = 350, Sum = 350},
            new Product_Sale { Number = 3, Name = "Корм для кошек", Quantity = 3, Unit= "кг.", Price = 120, Sum = 360},
            new Product_Sale { Number = 4, Name = "Наполнитель", Quantity = 1, Unit= "кг.", Price = 800, Sum = 800 },


            };


            SaleGrid.ItemsSource = products;
        }
        public class Product_Sale
        {
            public int Number { get; set; }
            public string Name { get; set; }
            public string Category { get; set; }
            public int Quantity { get; set; }
            public string Unit{ get; set; }
            public decimal Price { get; set; }
            public int Sum { get; set; }
        }

    }
}

