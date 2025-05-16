using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.Windows.Controls;
using static Home.Pages.Storage;

namespace Home.Pages
{
    public partial class Reports : Page
    {
        public SeriesCollection PieSeries { get; set; }

        public Reports()
        {
            InitializeComponent();

            PieSeries = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Ошейник для собак",
                    Values = new ChartValues<double> { 8 },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Корм для кошек",
                    Values = new ChartValues<double> { 12 },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Наполнитель CatStep",
                    Values = new ChartValues<double> { 7 },
                    DataLabels = true
                }
            };

            DataContext = this;

            var product = new List<Product_Rep>
    {
            new Product_Rep { Name = "Ошейник для собак",Category = "Аксессуары", Quantity = 8,  MIN = 5},
            new Product_Rep { Name = "Корм для кошек",Category = "Корма", Quantity = 12, MIN = 5},
            new Product_Rep {Name = "Наполнитель CatStep",Category = "Гигиена", Quantity = 7, MIN = 15 },


        };


            Product_Reports.ItemsSource = product;
        }
        public class Product_Rep
        {
            public string Name { get; set; }
            public string Category { get; set; }
            public int Quantity { get; set; }
            public int MIN { get; set; }
        }
    }

    
}
