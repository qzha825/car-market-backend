namespace CarMarketBackend.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; } = "";
        public string Model { get; set; } = "";
        public int Year { get; set; }
        public int Mileage { get; set; }
        public int Price { get; set; }
        public string Image { get; set; } = "";
    }
}
