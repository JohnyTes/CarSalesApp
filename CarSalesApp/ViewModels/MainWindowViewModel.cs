using CarSalesApp.Commands;
using CarSalesApp.Models;
using CarSalesApp.Services;
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
    public class MainWindowViewModel
    {
        public ObservableCollection<CarSalesSummary> SalesSummary { get; set; }
        public ICommand LoadXmlCommand { get; }
        private ICarLoader loader;
        private ISalesCalculator calculator;

        public MainWindowViewModel(ICarLoader loader,ISalesCalculator calculator)
        {
            this.loader = loader;
            this.calculator = calculator;

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
                    List<Car> cars = loader.Load(openFileDialog.FileName);

                    List<CarSalesSummary> salesSummary = calculator.GetWeekendSales(cars);

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
    }
}
