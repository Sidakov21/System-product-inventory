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

namespace System_product_inventory.Pages
{
    /// <summary>
    /// Логика взаимодействия для StatisticsPage.xaml
    /// </summary>
    public partial class StatisticsPage : Page
    {
        public StatisticsPage()
        {
            InitializeComponent();
            LoadProducts();
            UpdateSummary();
        }

        private void LoadProducts()
        {
            using (var db = new Entities())
            {
                var statistyc = db.StatisticsView.Select(sv => new
                {
                    sv.ID,
                    sv.Name,
                    sv.TotalProducts,
                    sv.AveragePrice,
                    sv.TotalValue
                }).ToList();

                // Привязываем к DataGrid
                StatistycsGrid.ItemsSource = statistyc;
            }

            UpdateSummary();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchBox.Text.Trim().ToLower();

            using (var db = new Entities())
            {
                var statistyc = db.StatisticsView.Select(sv => new
                {
                    sv.ID,
                    sv.Name,
                    sv.TotalProducts,
                    sv.AveragePrice,
                    sv.TotalValue
                }).Where(p => p.Name.ToLower().Contains(searchText)).ToList();
                // Привязываем к DataGrid
                StatistycsGrid.ItemsSource = statistyc;
            }

            UpdateSummary();
        }

        private void UpdateSummary()
        {
            decimal totalSum = 0;
            decimal averagePrice = 0;

            var items = StatistycsGrid.ItemsSource as IEnumerable<dynamic>;

            if (items != null && items.Any())
            {
                totalSum = items.Sum(i => (decimal)i.TotalValue);
                averagePrice = items.Average(i => (decimal)i.AveragePrice);
            }

            TotalSumText.Text = $"{totalSum:N2} ₽";
            AveragePriceText.Text = $"{averagePrice:N2} ₽";
        }

    }
}
