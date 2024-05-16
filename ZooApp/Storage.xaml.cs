﻿using Npgsql;
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
    /// Логика взаимодействия для Storage.xaml
    /// </summary>
    public partial class Storage : Window
    {
        public ObservableCollection<Fruit> Items { get; set; }
        public ICollectionView ItemsView { get; set; }
        public Storage()
        {
            InitializeComponent();


            Items = new ObservableCollection<Fruit>();

            LoadDataFromDatabase();

            ItemsView = CollectionViewSource.GetDefaultView(Items);
            DataGrid0.ItemsSource = ItemsView;
        }

        private void LoadDataFromDatabase()
        {
            NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; User Id=" + Databases.username + "; Password=" + Databases.password + "; Database=postgres");


            conn.Open();
            string query = "SELECT name, color FROM fruits"; // Замените на вашу таблицу и колонки
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


        private void DataGrid0_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

       
    }
    
}
