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
    /// Логика взаимодействия для AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window

    {
        private List<string> _categories;
        public AddProductWindow()
        {
            InitializeComponent();
            using (var db = new DatabaseService())
            {
                db.InitializeConnection(Databank.username, Databank.password);
                _categories = db.GetAllCategories();
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
            var categoryBox = CreateLabeledComboBox("Категория", _categories.ToArray(), 120);
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
            wrapper.Children.Add(categoryBox);
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
            var newItems = new List<StorageItem>();

            foreach (StackPanel wrapper in ItemsPanel.Children)
            {
                if (wrapper is StackPanel sp && sp.Children.Count >= 6)
                {

                    string name = ((sp.Children[0] as StackPanel)?.Children[1] as TextBox)?.Text;
                    string category = ((sp.Children[1] as StackPanel)?.Children[1] as ComboBox)?.SelectedItem?.ToString();
                    string unit = ((sp.Children[2] as StackPanel)?.Children[1] as ComboBox)?.SelectedItem?.ToString();
                    string quantityStr = ((sp.Children[3] as StackPanel)?.Children[1] as TextBox)?.Text;
                    string purchaseStr = ((sp.Children[4] as StackPanel)?.Children[1] as TextBox)?.Text;
                    string saleStr = ((sp.Children[6] as StackPanel)?.Children[1] as TextBlock)?.Text;


                    if (string.IsNullOrWhiteSpace(name) ||
                        string.IsNullOrWhiteSpace(category) ||
                        string.IsNullOrWhiteSpace(unit) ||
                        !int.TryParse(quantityStr, out int quantity) ||
                        !decimal.TryParse(purchaseStr, out decimal purchasePrice) ||
                        !decimal.TryParse(saleStr, out decimal salePrice))
                    {
                        MessageBox.Show("Проверьте введённые данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    newItems.Add(new StorageItem
                    {
                        Name = name,
                        Category = category,
                        Unit = unit,
                        Quantity = quantity,
                        PurchasePrice = purchasePrice,
                        Price = salePrice,

                        
                    });
                }

            }

            // Сохраняем в БД
            using (var db = new DatabaseService())
            {
                db.InitializeConnection(Databank.username, Databank.password);
                foreach (var item in newItems)
                {
                    db.AddStorageItem(item); 
                }
            }

            MessageBox.Show("Товары успешно добавлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            this.DialogResult = true;
            this.Close();
        }
    }
        
    
}

    

