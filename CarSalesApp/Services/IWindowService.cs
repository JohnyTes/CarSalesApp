using CarSalesApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarSalesApp.Services
{
    public interface IWindowService
    {
        void ShowDetailWindow(List<Car> cars);
    }
}
