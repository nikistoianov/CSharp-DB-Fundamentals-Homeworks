using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace P03_FootballBetting.Data.Models
{
    public class Country
    {

        public Country()
        {
            this.Teams = new HashSet<Team>();
            this.Towns = new HashSet<Town>();
        }

        [Key]
        public int CountryId { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Team> Teams { get; set; }
        public ICollection<Town> Towns { get; set; }
    }
}
