using System;
using System.Collections.Generic;
using System.Text;

namespace CarSalesApp.Models
{
    class CarDataGrid
    {
        public string Model { get; set; }
        public double PriceWithoutVAT { get; set; }
        public double PriceWithVAT { get; set; }
    }
}
