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
    /// Логика взаимодействия для AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        public AddProductWindow()
        {
            InitializeComponent();
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
            var unitBox = CreateLabeledComboBox("Ед. изм.", new[] { "шт.", "кг." }, 80);
            var quantityBox = CreateLabeledTextBox("Кол-во", 60);
            var purchaseBox = CreateLabeledTextBox("Цена закупки", 80);
            var markupBox = CreateLabeledTextBox("Наценка (%)", 80);
            var saleBlock = CreateLabeledTextBlock("Цена продажи", 100);

            // Получаем доступ к полям
            var purchaseTextBox = purchaseBox.Children[1] as TextBox;
            var markupTextBox = markupBox.Children[1] as TextBox;
            var saleTextBlock = saleBlock.Children[1] as TextBlock;

            // Пересчет цены
            purchaseTextBox.TextChanged += (s, ev) => UpdatePrice(purchaseTextBox, markupTextBox, saleTextBlock);
            markupTextBox.TextChanged += (s, ev) => UpdatePrice(purchaseTextBox, markupTextBox, saleTextBlock);

            // Кнопка удаления
            var deleteButton = new Button
            {
                Content = "🗑",
                Background = Brushes.Red,
                Foreground = Brushes.White,
                Width = 30,
                Height = 30,
                Margin = new Thickness(5, 20, 0, 0)
            };
            deleteButton.Click += (s, ev) => ItemsPanel.Children.Remove(wrapper);

            // Добавляем всё в wrapper
            wrapper.Children.Add(nameBox);
            wrapper.Children.Add(unitBox);
            wrapper.Children.Add(quantityBox);
            wrapper.Children.Add(purchaseBox);
            wrapper.Children.Add(markupBox);
            wrapper.Children.Add(saleBlock);
            wrapper.Children.Add(deleteButton);

            ItemsPanel.Children.Add(wrapper);
        }

        private void UpdatePrice(TextBox purchaseBox, TextBox markupBox, TextBlock resultBlock)
        {
            if (decimal.TryParse(purchaseBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price) &&
                decimal.TryParse(markupBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal markup))
            {
                var result = price + (price * markup / 100);
                resultBlock.Text = result.ToString("0.00");
            }
            else
            {
                resultBlock.Text = "0";
            }
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
            MessageBox.Show("Загрузка в БД будет реализована позже", "Инфо", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}

    

