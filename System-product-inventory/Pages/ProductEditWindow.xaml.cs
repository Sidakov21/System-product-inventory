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

namespace System_product_inventory.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductEditWindow.xaml
    /// </summary>
    public partial class ProductEditWindow : Window
    {
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public int Price { get; private set; }
        public int CategoryId { get; private set; }

        public ProductEditWindow(Product product = null)
        {
            InitializeComponent();

            using (var db = new Entities())
            {
                CategoryComboBox.ItemsSource = db.Category.ToList();
                CategoryComboBox.DisplayMemberPath = "Name";
                CategoryComboBox.SelectedValuePath = "Id";
            }

            if (product != null)
            {
                NameBox.Text = product.Name;
                QuantityBox.Text = product.Quantity.ToString();
                PriceBox.Text = product.Price.ToString();
                CategoryComboBox.SelectedValue = product.CategoryId;
            }

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(NameBox.Text) ||
                           CategoryComboBox.SelectedValue == null ||
                           !int.TryParse(QuantityBox.Text, out int quantity) ||
                           !int.TryParse(PriceBox.Text, out int price))
            {
                MessageBox.Show("Проверьте правильность введённых данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ProductName = NameBox.Text.Trim();
            Quantity = quantity;
            Price = price;
            CategoryId = (int)CategoryComboBox.SelectedValue;

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
