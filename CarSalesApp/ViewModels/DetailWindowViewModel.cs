using CarSalesApp.Models;

namespace CarSalesApp.ViewModels
{
    public class DetailWindowViewModel
    {
        public List<Car> Cars { get; }
        public string Title => $"Prodeje modelu: {Cars.FirstOrDefault()?.Model}";
        public DetailWindowViewModel(List<Car> cars)
        {
            Cars = cars;
        }
    }
}
