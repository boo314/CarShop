namespace AKarbowski.Infrastructure.Models
{
    public class CarModel
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
        public string CarType { get; set; }
        public string DriveType { get; set; }
        public int EnginePower { get; set; }
        public string Transmission { get; set; }
        public string Color { get; set; }

        public string CarCode { get; set; }

        public CarCategoryModel CarCategory { get; set; }
    }
}