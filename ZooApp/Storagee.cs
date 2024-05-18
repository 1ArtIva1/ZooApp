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
using System.Windows.Media;

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
            string query = "SELECT name, wholesale_price FROM catalog";
            using (var command = new NpgsqlCommand(query, conn))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    double pricedouble = reader.GetDouble(1);
                    string converterToStringForPrice = pricedouble.ToString();

                    var fruit = new Fruit
                    {
                        Name = reader.GetString(0),
                        Price = converterToStringForPrice,
                        

                    };
                    Items.Add(fruit);
                }
            }

        }


    }
}
