using CarSalesApp.Models;
using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace CarSalesApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void loadXml_click(object sender, RoutedEventArgs e)
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

                    List<CarDataGrid> carsDataGrid = GetWeekendSales(cars);

                    xmlGrid.ItemsSource = carsDataGrid;
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Failed to load XML file:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private List<Car> LoadCars(string filePath)
        {
            XmlDocument doc = new XmlDocument();

            doc.Load(filePath);

            return doc.GetElementsByTagName("Car")
                .Cast<XmlNode>()
                .Select(carNode => new Car
                {
                    Model = carNode["Model"]?.InnerText,
                    SaleDate = DateTime.Parse(carNode["SaleDate"]?.InnerText),
                    Price = double.Parse(carNode["Price"]?.InnerText),
                    VAT = double.Parse(carNode["VAT"]?.InnerText)
                })
                .ToList();
        }

        private List<CarDataGrid> GetWeekendSales(List<Car> cars)
        {
            return cars
                .Where(car =>
                    car.SaleDate.DayOfWeek == DayOfWeek.Saturday ||
                    car.SaleDate.DayOfWeek == DayOfWeek.Sunday)
                .GroupBy(car => car.Model)
                .Select(group => new CarDataGrid
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