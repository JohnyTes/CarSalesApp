using CarSalesApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarSalesApp.Services
{
    public interface ICarLoader
    {
        List<Car> Load(string filePath);
    }
}
