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
using Microsoft.Data;
using Microsoft.Data.Sql;

namespace System_product_inventory.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        public ProductPage()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            using (var db = new Entities())
            {
                //var products = (from p in db.Product
                //                join c in db.Category on p.CategoryId equals c.Id into pc
                //                from c in pc.DefaultIfEmpty()
                //                select new Product
                //                {
                //                    Name = p.Name,
                //                    Category = c.Name,
                //                    Quantity = p.Quantity,
                //                    Price = p.Price
                //                }).ToList();

                // Привязываем к DataGrid
                ProductsGrid.ItemsSource = products;
            }
        }

    }
}
