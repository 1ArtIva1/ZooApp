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
        public Storage()
        {
            InitializeComponent();
            var products = new List<Product>
    {
            new Product { Name = "Игрушка для собак", Quantity = 15, Category = "Аксессуары", Price = 350 },
            new Product { Name = "Игрушка для кошек", Quantity = 10, Category = "Аксессуары", Price = 350 },
            new Product { Name = "Игрушка для хомячков", Quantity = 30, Category = "Аксессуары", Price = 350 },
            new Product { Name = "Корм для собак", Quantity = 20, Category = "Аксессуары", Price = 1000 },
            new Product { Name = "Корм для кошек", Quantity = 15, Category = "Корма", Price = 120 },
            new Product { Name = "Наполнитель", Quantity = 8, Category = "Гигиена", Price = 800 },
      
        
    };

            
            myDataGrid.ItemsSource = products;
        }
        public class Product
        {
            public string Name { get; set; }
            public int Quantity { get; set; }
            public string Category { get; set; }
            public decimal Price { get; set; }
        }


    }
}
