using CarSalesApp.Models;

namespace CarSalesApp.Services
{
    public interface ISalesCalculator
    {
        List<CarSalesSummary> GetWeekendSales(List<Car> cars);
    }
}
