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
using Home.Service;
using Home.Services;

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

            using (var db = new DatabaseService("localhost", 5432, "vkr"))
            {
                db.InitializeConnection(Databank.username, Databank.password);
                Databank.StorageItems = db.LoadStorageItems();
            }
            myDataGrid.ItemsSource = Databank.StorageItems;
        }
    }
}
