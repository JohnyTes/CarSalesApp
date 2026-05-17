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
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "XML files (*.xml)|*.xml",
                Title = "Open XML File"
            };

            bool? result = ofd.ShowDialog();
            if (result == true)
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(ofd.FileName);
                    List<Car> cars = doc.GetElementsByTagName("Car").Cast<XmlNode>()
                           .Select(x => new Car
                           {
                               Model = x["Model"]?.InnerText,
                               SaleDate = DateTime.Parse(x["SaleDate"]?.InnerText),
                               Price = double.Parse(x["Price"]?.InnerText),
                               VAT = double.Parse(x["VAT"]?.InnerText)
                           })
                           .ToList();

                    List<Car> weekendCars = cars
                        .Where(car => car.SaleDate.DayOfWeek == DayOfWeek.Saturday || car.SaleDate.DayOfWeek == DayOfWeek.Sunday)
                        .ToList();

                    List<CarDataGrid> carsDataGrid = weekendCars.GroupBy(car => car.Model)
                        .Select(x => new CarDataGrid
                        {
                            Model = x.Key,
                            PriceWithoutVAT = x.Sum(car => car.Price),
                            PriceWithVAT = x.Sum(car => car.Price * (1 + car.VAT / 100))
                        })
                        .ToList();

                    xmlGrid.ItemsSource = carsDataGrid;
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Failed to load XML file:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}