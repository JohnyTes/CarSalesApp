using CarSalesApp.Models;

namespace CarSalesApp.Services
{
    public interface ICarLoader
    {
        List<Car> Load(string filePath);
    }
}
