using MaterialDesignThemes.Wpf;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ZooApp
{
    internal class Storagee
    {

        public ObservableCollection<Fruit> Items { get; set; }
        public ICollectionView ItemsView { get; set; }

        public Storagee()
        {

            Items = new ObservableCollection<Fruit>();
            ItemsView = CollectionViewSource.GetDefaultView(Items);

        }
        public void LoadDataFromDatabase()
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


    }
}
