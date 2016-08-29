using AKarbowski.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AKarbowski.Infrastructure.ViewModels
{
    public class CarDetailsViewModel
    {
        public int CarId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Version { get; set; }
        public int YearOfProduction { get; set; }
        public int Mileage { get; set; }
        public double EngineCapacity { get; set; }
        public string FuelType { get; set; }
        public int AmountOfDoors { get; set; }
        public int AmountOfSeats { get; set; }
        public int EnginePower { get; set; }
        public string Color { get; set; }
        public string CarCode { get; set; }

        public string Category { get; set; }
        public string Type { get; set; }
        public List<ImageViewModel> Images { get; set; }
    }
}