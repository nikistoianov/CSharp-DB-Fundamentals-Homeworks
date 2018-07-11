using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace P03_SalesDatabase.Data.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public double Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }
    }
}
