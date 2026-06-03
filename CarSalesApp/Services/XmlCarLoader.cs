using CarSalesApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace CarSalesApp.Services
{
    internal class XmlCarLoader
    {
        public List<Car> Load(string filePath)
        {
            XDocument doc = XDocument.Load(filePath);

            return doc.Descendants("Car")
                .Select(car =>
                {
                    string? model = car.Element("Model")?.Value;
                    string? saleDateValue = car.Element("SaleDate")?.Value;
                    string? priceValue = car.Element("Price")?.Value;
                    string? vatValue = car.Element("VAT")?.Value;

                    if (string.IsNullOrWhiteSpace(model) ||
                        !DateTime.TryParse(saleDateValue, out DateTime saleDate) ||
                        !double.TryParse(priceValue, out double price) ||
                        !double.TryParse(vatValue, out double vat))
                    {
                        throw new Exception("Invalid XML structure.");
                    }

                    return new Car
                    {
                        Model = model,
                        SaleDate = saleDate,
                        Price = price,
                        VAT = vat
                    };
                })
                .ToList();
        }
    }
}
