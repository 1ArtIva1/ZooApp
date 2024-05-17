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
        public ObservableCollection<Fruit> Items { get; set; }
        public ICollectionView ItemsView { get; set; }



        public ObservableCollection<Fruit> Items2 { get; set; }
        public ICollectionView ItemsView2 { get; set; }
        public ArrangeSellForm()
        {
            InitializeComponent();
            Time time = new Time();
            time.Timer_Day(label1);
            time.Timer_Clock(label2);
            time.Timer_Data(label3);

            Items = new ObservableCollection<Fruit>();

            Items2 = new ObservableCollection<Fruit>();

            ItemsView = CollectionViewSource.GetDefaultView(Items);

            ItemsView2 = CollectionViewSource.GetDefaultView(Items2);



            LoadDataFromDatabase();

            DataGrid0.ItemsSource = ItemsView;


        }
        private void LoadDataFromDatabase()
        {
            NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; User Id=" + Databases.username + "; Password=" + Databases.password + "; Database=postgres");


            conn.Open();
            string query = "SELECT name, color FROM fruits";
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
                    Items.Add(fruit);
                }
            }

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

        private void DataGrid0_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGrid0.SelectedItem != null)
            {
                var selectedItem = DataGrid0.SelectedItem as Fruit; // Замените YourDataType на тип данных вашей модели
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

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            Storage storage = new Storage();

            storage.Show();
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }

        public void Update()
        {
            Items2.Clear();

            LoadDataFromDatabase2();

            DataGrid3.ItemsSource = ItemsView2;
        }
        
    }
}
