using CarSalesApp.Models;

namespace CarSalesApp.Services
{
    public class SalesCalculator : ISalesCalculator
    {
        public List<CarSalesSummary> GetWeekendSales(List<Car> cars)
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
