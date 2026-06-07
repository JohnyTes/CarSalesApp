using CarSalesApp.Commands;
using CarSalesApp.Models;
using CarSalesApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;

namespace CarSalesApp.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        public ObservableCollection<CarSalesSummary> SalesSummary { get; set; }

        private ICarLoader loader;
        private ISalesCalculator calculator;
        private IWindowService windowService;

        private List<Car> loadedCars = new();
        [ObservableProperty]
        private CarSalesSummary? selectedSummary;

        public MainWindowViewModel(ICarLoader loader, ISalesCalculator calculator, IWindowService windowService)
        {
            this.loader = loader;
            this.calculator = calculator;
            this.windowService = windowService;

            SalesSummary = new ObservableCollection<CarSalesSummary>();
        }

        [RelayCommand]
        private async Task LoadXml()
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
                    loadedCars = await Task.Run(() => loader.Load(openFileDialog.FileName));

                    List<CarSalesSummary> salesSummary = await Task.Run(() => calculator.GetWeekendSales(loadedCars));

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

        [RelayCommand]
        private void SelectSummary(object? parameter)
        {
            if (parameter is CarSalesSummary summary)
            {
                SelectedSummary = summary;
                List<Car> carsForModel = loadedCars
                    .Where(car => car.Model == summary.Model)
                    .ToList();

                windowService.ShowDetailWindow(carsForModel);
            }
        }
    }
}
