using CarSalesApp.Models;
using CarSalesApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace CarSalesApp.Services
{
    public class WindowService : IWindowService
    {
        public void ShowDetailWindow(List<Car> cars)
        {
            DetailWindowViewModel viewModel = new DetailWindowViewModel(cars);
            Window window = new DetailWindow(viewModel);
            window.Show();
        }
    }
}
