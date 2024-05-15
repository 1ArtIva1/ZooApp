using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace ZooApp
{
    /// <summary>
    /// Логика взаимодействия для OrderForm.xaml
    /// </summary>
    public partial class OrderForm : Window
    {
        public ObservableCollection<Fruit> Items { get; set; }
        public ICollectionView ItemsView { get; set; }


        public OrderForm()
        {
            InitializeComponent();



            Time time = new Time();
            time.Timer_Day(label1);
            time.Timer_Clock(label2);
            time.Timer_Data(label3);
            Items = new ObservableCollection<Fruit>
            {
                new Fruit { Name = "Apple", Color = "Red" },
                new Fruit { Name = "Banana", Color = "Yellow" },
                new Fruit { Name = "Cherry", Color = "Red" },
                new Fruit { Name = "Date", Color = "Brown" },
                new Fruit { Name = "Elderberry", Color = "Black" },
                new Fruit { Name = "Fig", Color = "Purple" },
                new Fruit { Name = "Grape", Color = "Green" },
                new Fruit { Name = "Honeydew", Color = "Green" }
            };

            ItemsView = CollectionViewSource.GetDefaultView(Items);
            DataGrid3.ItemsSource = ItemsView;
        }
      
        private void SearchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var searchText = SearchTextBox.Text.ToLower();
            ItemsView.Filter = item =>
            {
                if (string.IsNullOrEmpty(searchText))
                {
                    return true;
                }
                var fruit = item as Fruit;
                return fruit.Name.ToLower().Contains(searchText) || fruit.Color.ToLower().Contains(searchText);
            };
            ItemsView.Refresh();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainSeller mainSeller = new MainSeller();
            mainSeller.Show();
            this.Close();
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
