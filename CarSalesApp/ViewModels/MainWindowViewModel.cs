using CarSalesApp.Commands;
using CarSalesApp.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace CarSalesApp.ViewModels
{
    internal class MainWindowViewModel
    {
        public ObservableCollection<CarSalesSummary> SalesSummary { get; set; }
        public ICommand LoadXmlCommand { get; }

        public MainWindowViewModel()
        {
            SalesSummary = new ObservableCollection<CarSalesSummary>();
            LoadXmlCommand = new RelayCommand(LoadXml);
        }

        private void LoadXml()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "XML files (*.xml)|*.xml",
                Title = "Open XML File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    List<Car> cars = LoadCars(openFileDialog.FileName);

                    List<CarSalesSummary> salesSummary = GetWeekendSales(cars);

                    SalesSummary.Clear();

                    foreach (var summary in salesSummary)
                    {
                        SalesSummary.Add(summary);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to load XML file:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private List<Car> LoadCars(string filePath)
        {
            XDocument doc = XDocument.Load(filePath);



            return doc.Descendants("Car")
                .Select(car =>
                {
                    string? model = car.Element("Model")?.Value;
                    string? saleDateValue = car.Element("SaleDate")?.Value;
                    string? priceValue = car.Element("Price")?.Value;
                    string? vatValue = car.Element("VAT")?.Value;

                    if (string.IsNullOrWhiteSpace(model) ||
                        !DateTime.TryParse(saleDateValue, out DateTime saleDate) ||
                        !double.TryParse(priceValue, out double price) ||
                        !double.TryParse(vatValue, out double vat))
                    {
                        throw new Exception("Invalid XML structure.");
                    }

                    return new Car
                    {
                        Model = model,
                        SaleDate = saleDate,
                        Price = price,
                        VAT = vat
                    };
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
