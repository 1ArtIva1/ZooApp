using Microsoft.SqlServer.Server;
using Npgsql;
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

namespace ZooApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class LoginForm : Window
    {
        public LoginForm()
        {
            InitializeComponent();

        }

        private void Button_Exit(object sender, RoutedEventArgs e)
        {
            this.Close();

        }

        private void Button_Click_Login(object sender, RoutedEventArgs e)
        {
            bool flag = false;
            Databases.username = loginTextBox.Text;
            Databases.password = passwordTextBox.Password;
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; User Id=" + Databases.username + "; Password=" + Databases.password + "; Database=postgres");
                conn.Open();
                if (flag) { conn.Close(); }
            }
            catch (NpgsqlException) { MessageBox.Show("Account doesn't exist!"); flag = true; }
            if (!flag)
            {
                switch (Databases.username)
                {
                    case "postgres":
                        MainManager tform = new MainManager();
                        tform.Show();
                        this.Close();
                        break;
                    default:
                        MainSeller sform = new MainSeller();
                        sform.Show();
                        this.Close();
                        break;
                }
                
            }

        }
    }
}
