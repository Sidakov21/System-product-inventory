using Microsoft.Data;
using Microsoft.Data.Sql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace System_product_inventory.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        private ObservableCollection<Product> _items = new ObservableCollection<Product>();

        public ProductPage()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            using (var db = new Entities())
            {
                var products = db.Product.Select(p => new
                {
                    p.Id,
                    p.Name,
                    CategoryName = db.Category
                                     .Where(c => c.Id == p.CategoryId)
                                     .Select(c => c.Name)
                                     .FirstOrDefault() ?? "NULL",
                    p.Quantity,
                    p.Price
                }).ToList();

                // Привязываем к DataGrid
                ProductsGrid.ItemsSource = products;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new ProductEditWindow();

            if (window.ShowDialog() == true)
            {
                using (var db = new Entities())
                {
                    var product = new Product
                    {
                        Name = window.ProductName,
                        Quantity = window.Quantity,
                        Price = window.Price,
                        CategoryId = window.CategoryId
                    };

                    db.Product.Add(product);
                    db.SaveChanges();
                }

                LoadProducts();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

            // Проверяем, выбран ли элемент
            if (ProductsGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите товар для редактирования.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Получаем выбранный товар
            dynamic selected = ProductsGrid.SelectedItem;
            int productId = selected.Id;

            using (var db = new Entities())
            {
                var product = db.Product.FirstOrDefault(p => p.Id == productId);
                if (product == null)
                {
                    MessageBox.Show("Товар не найден в базе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var editWindow = new ProductEditWindow(product);

                if (editWindow.ShowDialog() == true)
                {
                    // Обновляем данные товара
                    product.Name = editWindow.ProductName;
                    product.Quantity = editWindow.Quantity;
                    product.Price = editWindow.Price;
                    product.CategoryId = editWindow.CategoryId;

                    db.SaveChanges();
                }
            }

            // После закрытия окна обновляем таблицу
            LoadProducts();
        }
    }
}
