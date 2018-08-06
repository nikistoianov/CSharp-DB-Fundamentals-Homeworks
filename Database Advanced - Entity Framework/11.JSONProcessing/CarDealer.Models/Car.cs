namespace CarDealer.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Car
    {
        public Car()
        {
            this.PartsCars = new List<PartCar>();
            this.Sales = new List<Sale>();
        }

        public int Id { get; set; }

        [Required]
        public string Make { get; set; }

        [Required]
        public string Model { get; set; }

        public double TravelledDistance { get; set; }

        public ICollection<PartCar> PartsCars { get; set; }
        public ICollection<Sale> Sales { get; set; }
    }
}
