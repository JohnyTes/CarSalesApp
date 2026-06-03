using CarSalesApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarSalesApp.Services
{
    public interface ISalesCalculator
    {
        List<CarSalesSummary> GetWeekendSales(List<Car> cars);
    }
}
