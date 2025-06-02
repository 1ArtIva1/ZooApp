using Home.Service;
using Home.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Home.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddDiscountWindow.xaml
    /// </summary>
    public partial class AddDiscountWindow : Window
    {
        public AddDiscountWindow()
        {
            InitializeComponent();

            using (var db = new DatabaseService())
            {
                db.InitializeConnection(Databank.username, Databank.password);
            }
        }

        private void AddItemBlock_Click(object sender, RoutedEventArgs e)
        {
            var wrapper = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(0, 5, 0, 0)
            };

            var nameBox = CreateLabeledTextBox("Наименование", 120);
            var valueBox = CreateLabeledTextBox("Процент", 80);

            wrapper.Children.Add(nameBox);
            wrapper.Children.Add(valueBox);
            
            ItemsPanel.Children.Add(wrapper);

        }

        private StackPanel CreateLabeledTextBox(string label, double width)
        {
            return new StackPanel
            {
                Margin = new Thickness(5),
                Children =
                {
                    new TextBlock { Text = label, Foreground = Brushes.White, FontSize = 12 },
                    new TextBox { Width = width, Height = 25, FontSize = 12 }
                }
            };
        }

        private StackPanel CreateLabeledTextBlock(string label, double width)
        {
            return new StackPanel
            {
                Margin = new Thickness(5),
                Children =
                {
                    new TextBlock { Text = label, Foreground = Brushes.White, FontSize = 12 },
                    new TextBlock
                    {
                        Width = width,
                        Height = 25,
                        Background = Brushes.Gray,
                        Foreground = Brushes.White,
                        Text = "0",
                        FontSize = 12,
                        TextAlignment = TextAlignment.Center
                    }
                }
            };
        }

        private StackPanel CreateLabeledComboBox(string label, string[] options, double width)
        {
            var comboBox = new ComboBox
            {
                Width = width,
                Height = 25,
                FontSize = 12
            };

            foreach (var option in options)
                comboBox.Items.Add(option);

            comboBox.SelectedIndex = 0;

            return new StackPanel
            {
                Margin = new Thickness(5),
                Children =
                {
                    new TextBlock { Text = label, Foreground = Brushes.White, FontSize = 12 },
                    comboBox
                }
            };
        }

        private void SaveToDatabase_Click(object sender, RoutedEventArgs e)
        {
            var newDiscounts = new List<DiscountsList>();

            foreach (StackPanel wrapper in ItemsPanel.Children)
            {
                if (wrapper is StackPanel sp && sp.Children.Count >= 2)
                {
                    string name = ((sp.Children[0] as StackPanel)?.Children[1] as TextBox)?.Text;
                    string valueStr = ((sp.Children[1] as StackPanel)?.Children[1] as TextBox)?.Text;

                    if (string.IsNullOrWhiteSpace(name) ||
                        !int.TryParse(valueStr, out int value))
                    {
                        MessageBox.Show("Проверьте введённые данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    newDiscounts.Add(new DiscountsList
                    {
                        Name = name,
                        Value = value
                    });

                }
            }

            // Сохраняем в БД
            using (var db = new DatabaseService())
            {
                db.InitializeConnection(Databank.username, Databank.password);
                foreach (var discounts in newDiscounts)
                {
                    db.AddDiscounts(discounts);
                }
            }

            MessageBox.Show("Скидки успешно добавлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            this.DialogResult = true;
            this.Close();
        }

    }
}
