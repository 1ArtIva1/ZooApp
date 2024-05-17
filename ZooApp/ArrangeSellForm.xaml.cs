using MaterialDesignThemes.Wpf;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;



namespace ZooApp
{
    /// <summary>
    /// Логика взаимодействия для ArrangeSellForm.xaml
    /// </summary>
    public partial class ArrangeSellForm : Window
    {
       
        public ObservableCollection<Fruit> Items2 { get; set; }
        public ICollectionView ItemsView2 { get; set; }
        public ArrangeSellForm()
        {
            Storagee storageе = new Storagee();
            InitializeComponent();
            Time time = new Time();
            time.Timer_Day(label1);
            time.Timer_Clock(label2);
            time.Timer_Data(label3);

            storageе.Items = new ObservableCollection<Fruit>();
            Items2 = new ObservableCollection<Fruit>();

            storageе.ItemsView = CollectionViewSource.GetDefaultView(storageе.Items);
            ItemsView2 = CollectionViewSource.GetDefaultView(Items2);

            storageе.LoadDataFromDatabase();

            DataGrid0.ItemsSource = storageе.ItemsView;

        }

        private void SearchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)

        {
            Storage storage = new Storage();
            var searchText = SearchTextBox.Text.ToLower();
            storage.ItemsView.Filter = item =>
            {
                if (string.IsNullOrEmpty(searchText))
                {
                    return true;
                }
                var fruit = item as Fruit;
                return fruit.Name.ToLower().Contains(searchText) || fruit.Color.ToLower().Contains(searchText);
            };
            storage.ItemsView.Refresh();
        }

        private void DataGrid0_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGrid0.SelectedItem != null)
            {
                var selectedItem = DataGrid0.SelectedItem as Fruit; 
                if (selectedItem != null)
                {
                    InsertDataToDatabase(selectedItem);
                    Update();

                }
                
            }
        }
        private void InsertDataToDatabase(Fruit selectedItem)
        {
            NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; User Id=" + Databases.username + "; Password=" + Databases.password + "; Database=postgres");


            conn.Open();

            string query = "INSERT INTO fruits2 (Name, Color) VALUES (@value1, @value2)";

            using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@value1", selectedItem.Name); // Замените Property1 на соответствующее свойство вашей модели
                command.Parameters.AddWithValue("@value2", selectedItem.Color); // Замените Property2 на соответствующее свойство вашей модели

                command.ExecuteNonQuery();
            }
        }
        private void DataGrid0_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public void LoadDataFromDatabase2()
        {
            NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; User Id=" + Databases.username + "; Password=" + Databases.password + "; Database=postgres");


            conn.Open();
            string query = "SELECT name, color FROM fruits2";
            using (var command = new NpgsqlCommand(query, conn))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var fruit = new Fruit
                    {
                        Name = reader.GetString(0),
                        Color = reader.GetString(1)
                    };
                    Items2.Add(fruit);
                }
            }

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainSeller mainSeller = new MainSeller();
            mainSeller.Show();
            this.Close();
        }

        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            DataGrid0.Visibility = Visibility;
            SearchTextBox.Visibility = Visibility;
        }

        public void Update()
        {
            Items2.Clear();

            LoadDataFromDatabase2();

            DataGrid3.ItemsSource = ItemsView2;
        }
        
    }
}
