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
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<CarSalesSummary> SalesSummary { get; set; }
        public ICommand LoadXmlCommand { get; }
        public ICommand SelectSummaryCommand { get; }

        public CarSalesSummary? SelectedSummary
        {
            get => selectedSummary;
            set
            {
                if (selectedSummary != value)
                {
                    selectedSummary = value;
                    OnPropertyChanged();
                }
            }
        }

        private ICarLoader loader;
        private ISalesCalculator calculator;
        private IWindowService windowService;

        private List<Car> loadedCars = new();
        private CarSalesSummary? selectedSummary;

        public MainWindowViewModel(ICarLoader loader,ISalesCalculator calculator, IWindowService windowService)
        {
            this.loader = loader;
            this.calculator = calculator;
            this.windowService = windowService;

            SalesSummary = new ObservableCollection<CarSalesSummary>();
            LoadXmlCommand = new RelayCommand(async _ => await LoadXml());
            SelectSummaryCommand = new RelayCommand(SelectSummary);
        }

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
