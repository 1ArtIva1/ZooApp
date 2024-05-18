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
       
        // Коллекции
        public ObservableCollection<Fruit> Items2 { get; set; }
        public ICollectionView ItemsView2 { get; set; }
        public ArrangeSellForm()
        {
            
            InitializeComponent();
            //Подключаем объекты классов
            Storagee storageе = new Storagee();
            Time time = new Time();

            //Время и дата на форме
            time.Timer_Day(label1);
            time.Timer_Clock(label2);
            time.Timer_Data(label3);

            //Создаём коллекции
            storageе.Items = new ObservableCollection<Fruit>();
            Items2 = new ObservableCollection<Fruit>();

            storageе.ItemsView = CollectionViewSource.GetDefaultView(storageе.Items);
            ItemsView2 = CollectionViewSource.GetDefaultView(Items2);

            //Подгружаем БД и заполняем grid (склад)
            storageе.LoadDataFromDatabase();

            DataGrid0.ItemsSource = storageе.ItemsView;

        }

        //Метод для поисковой строки
        private void SearchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            Storagee storage = new Storagee();

            var searchText = SearchTextBox.Text.ToLower();
            storage.ItemsView.Filter = item =>
            {
                if (string.IsNullOrEmpty(searchText))
                {
                    return true;
                }
                var fruit = item as Fruit;
                return fruit.Name.ToLower().Contains(searchText);// || fruit.Price.ToString().ToLower().Contains(searchText);
            };
            storage.ItemsView.Refresh();
        }

        //Метод двойного нажатия по складу
        private void DataGrid0_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Refresh refresh = new Refresh();
            if (DataGrid0.SelectedItem != null)
            {
                var selectedItem = DataGrid0.SelectedItem as Fruit; 
                if (selectedItem != null)
                {
                    InsertDataToDatabase(selectedItem);
                    refresh.UpdateArrangeSellForm();

                }
                
            }
        }

        //Добавляем данные в промежуточную таблицу (тест)
        private void InsertDataToDatabase(Fruit selectedItem)
        {
            NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; User Id=" + Databases.username + "; Password=" + Databases.password + "; Database=postgres");


            conn.Open();

            string query = "INSERT INTO fruits2 (Name, Color) VALUES (@value1, @value2)";

            using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@value1", selectedItem.Name); 
                command.Parameters.AddWithValue("@value2", selectedItem.Price); 

                command.ExecuteNonQuery();
            }
        }

        private void DataGrid0_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //Загружаем БД для grid (чек)
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
                        Price= reader.GetString(1)
                    };
                    Items2.Add(fruit);
                }
            }

        }

        //Метод кнопки (Назад)
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainSeller mainSeller = new MainSeller();
            mainSeller.Show();
            this.Close();
        }

        //Метод ссылки (Добавить)
        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            DataGrid0.Visibility = Visibility;
            SearchTextBox.Visibility = Visibility;
            Label_Choose.Visibility = Visibility;
            Krestik.Visibility = Visibility;
            close_textbox.Visibility = Visibility;
        }

        //Метод обновления grid (Чек)
        //public void Update()
        //{
        //    Items2.Clear();

        //    LoadDataFromDatabase2();

        //    DataGrid3.ItemsSource = ItemsView2;
        //}

        //Метод закрытия grid (Склад)
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            DataGrid0.Visibility = Visibility.Hidden;
            SearchTextBox.Visibility = Visibility.Hidden;
            Label_Choose.Visibility = Visibility.Hidden;
            Krestik.Visibility = Visibility.Hidden;
            close_textbox.Visibility = Visibility.Hidden;   
        }

        //Метод очистки grid (чек)
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Refresh refresh = new Refresh();
            NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; User Id=" + Databases.username + "; Password=" + Databases.password + "; Database=postgres");
            conn.Open();
            string query = $"TRUNCATE TABLE fruits2;";
            using (var command = new NpgsqlCommand(query, conn))
            {
                command.ExecuteNonQuery();
            }
            refresh.UpdateArrangeSellForm();
            
        }
    }
}
