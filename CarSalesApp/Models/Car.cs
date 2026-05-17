using System;
using System.Collections.Generic;
using System.Text;

namespace CarSalesApp.Models
{
    public class Car
    {
        public string Model { get; set; }
        public DateTime SaleDate { get; set; }
        public double Price { get; set; }
        public double VAT { get; set; }
    }
}
