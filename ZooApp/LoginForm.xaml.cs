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
using ZooApp.Services;
using System.Data;

namespace ZooApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class LoginForm : Window
    {
        private readonly DatabaseService _dbService;

        public LoginForm()
        {
            InitializeComponent();

            _dbService = new DatabaseService("localhost", 5432, "postgres");

        }

        private void Button_Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_Login(object sender, RoutedEventArgs e)
        {
            Databank.username = loginTextBox.Text;
            Databank.password = passwordTextBox.Password;

            try
            {
                _dbService.InitializeConnection(Databank.username, Databank.password);
                _dbService.OpenConnection();

                // Получаем список ролей пользователя
                List<string> userRoles = _dbService.GetUserRoles();

                // Проверяем принадлежность к группе emps
                bool isEmp = userRoles.Contains("emps");
                bool isManager = userRoles.Contains("managers");

                _dbService.CloseConnection();

                // Открываем соответствующую форму
                
                if (isManager)
                {
                    MainManager tform = new MainManager();
                    tform.Show();
                    this.Close();
                }
                else if (isEmp)
                {
                    MainSeller sform = new MainSeller();
                    sform.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show($"Не назначена роль пользователя",
                              "Авторизация",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
                }
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show($"Пользователь не зарегистрирован в системе",
                              "Авторизация",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }

            //bool flag = false;

            //Databank.username = loginTextBox.Text;
            //Databank.password = passwordTextBox.Password;
            //
            //try
            //{
            //    _dbService.InitializeConnection(Databank.username, Databank.password);
            //
            //    _dbService.OpenConnection();
            //    _dbService.CloseConnection();
            //
            //
            //    //MainSeller sform = new MainSeller();
            //    //sform.Show();
            //    //this.Close();
            //
            //}
            //catch (NpgsqlException ex)
            //{
            //    MessageBox.Show("Ошибка авторизации", "Ошибка",
            //              MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            //
            //var userRoles = _dbService.GetUserRoles();
            //
            //if (userRoles.Contains("emps"))
            //{
            //    // Пользователь в группе emps
            //    MainSeller sform = new MainSeller();
            //    sform.Show();
            //}
            //else if (userRoles.Contains("managers"))
            //{
            //    // Пользователь в группе managers
            //    MainManager tform = new MainManager();
            //    tform.Show();
            //}
            //else
            //{
            //    // Пользователь не входит ни в одну из известных групп
            //    MessageBox.Show("Доступ запрещен. Недостаточно прав.");
            //    Application.Current.Shutdown();
            //}
        }

        public DatabaseService GetAuthenticatedDbService()
        {
            return _dbService;
        }

        //Databases.username = loginTextBox.Text;
        //Databases.password = passwordTextBox.Password;
        //try
        //{
        //    NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; User Id=" + Databases.username + "; Password=" + Databases.password + "; Database=postgres");
        //    conn.Open();
        //    if (flag) { conn.Close(); }
        //}
        //catch (NpgsqlException) { MessageBox.Show("Account doesn't exist!"); flag = true; }
        //if (!flag)
        //{
        //    switch (Databases.username)
        //    {
        //        case "postgres":
        //            MainManager tform = new MainManager();
        //            tform.Show();
        //            this.Close();
        //            break;
        //        default:
        //            MainSeller sform = new MainSeller();
        //            sform.Show();
        //            this.Close();
        //            break;
        //    }
        //
        //}


    }
}
