using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ZooApp
{
    internal class Refresh
    {
        public void UpdateArrangeSellForm()
        {
            ArrangeSellForm arrangeSellForm = new ArrangeSellForm();
            arrangeSellForm.Items2.Clear();
            arrangeSellForm.LoadDataFromDatabase2();

            arrangeSellForm.DataGrid3.ItemsSource = arrangeSellForm.ItemsView2;
        }
    }
}
