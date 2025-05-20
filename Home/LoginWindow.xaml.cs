using Home.Services;
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
using System.Windows.Shapes;

namespace Home
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly DatabaseService _dbService;

        public LoginWindow()
        {
            InitializeComponent();

            _dbService = new DatabaseService("localhost", 5432, "postgres");
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            Databank.username = loginBox.Text;
            Databank.password = passwordBox.Password;

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
                    AdminWindow sform = new AdminWindow();
                    sform.Show();
                    this.Close();
                }
                else if (isEmp)
                {
                    MainWindow sform = new MainWindow();
                    sform.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Не назначена роль пользователя",
                              "Авторизация",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
                }

            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Пользователь не зарегистрирован в системе",
                              "Авторизация",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }

        }

        public DatabaseService GetAuthenticatedDbService()
        {
            return _dbService;
        }

    }
}
