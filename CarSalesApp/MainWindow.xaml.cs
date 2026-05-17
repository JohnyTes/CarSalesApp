using CarSalesApp.Models;
using Microsoft.Win32;
using System.Windows;
using System.Xml.Linq;

namespace CarSalesApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadXml_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "XML files (*.xml)|*.xml",
                Title = "Open XML File"
            };

            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                try
                {
                    List<Car> cars = LoadCars(openFileDialog.FileName);

                    List<CarSalesSummary> carsDataGrid = GetWeekendSales(cars);

                    carsItemsControl.ItemsSource = carsDataGrid;
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Failed to load XML file:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private List<Car> LoadCars(string filePath)
        {
            XDocument doc = XDocument.Load(filePath);

            return doc.Descendants("Car")
                .Select(carNode => new Car
                {
                    Model = carNode.Element("Model")?.Value,
                    SaleDate = DateTime.Parse(carNode.Element("SaleDate")?.Value),
                    Price = double.Parse(carNode.Element("Price")?.Value),
                    VAT = double.Parse(carNode.Element("VAT")?.Value)
                })
                .ToList();
        }

        private List<CarSalesSummary> GetWeekendSales(List<Car> cars)
        {
            return cars
                .Where(car =>
                    car.SaleDate.DayOfWeek == DayOfWeek.Saturday ||
                    car.SaleDate.DayOfWeek == DayOfWeek.Sunday)
                .GroupBy(car => car.Model)
                .Select(group => new CarSalesSummary
                {
                    Model = group.Key,
                    PriceWithoutVAT = group.Sum(car => car.Price),
                    PriceWithVAT = group.Sum(car =>
                        car.Price * (1 + car.VAT / 100))
                })
                .ToList();
        }
    }
}