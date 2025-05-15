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

        public ObservableCollection<StorageCall> Items { get; set; }
        public ICollectionView ItemsView { get; set; }

        public Storagee()
        {

            Items = new ObservableCollection<StorageCall>();
            ItemsView = CollectionViewSource.GetDefaultView(Items);

        }
        public void LoadDataFromDatabase()
        {
            NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; User Id=" + Databank.username + "; Password=" + Databank.password + "; Database= zoo");


            conn.Open();
            string query = "SELECT * FROM get_name_products();";
            using (var command = new NpgsqlCommand(query, conn))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    //double pricedouble = reader.GetDouble(1);
                    //string converterToStringForPrice = pricedouble.ToString();

                    var fruit = new StorageCall
                    {
                        Name = reader.GetString(0),
                        Category = reader.GetString(1),
                        SubCategory = reader.GetString(2),
                        Price = reader.GetDouble(3),
                        Qty = reader.GetInt32(4)

                    };
                    Items.Add(fruit);
                }
            }

        }


    }
}
