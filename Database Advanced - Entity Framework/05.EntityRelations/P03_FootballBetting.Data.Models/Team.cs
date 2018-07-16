using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P03_FootballBetting.Data.Models
{
    public class Team
    {
        public Team()
        {
            this.Players = new HashSet<Player>();
            this.HomeGames = new HashSet<Game>();
            this.AwayGames = new HashSet<Game>();
        }
        
        [Key]
        public int TeamId { get; set; }

        [Required]
        public string Name { get; set; }

        public string LogoUrl { get; set; }

        [Required]
        [MaxLength(3)]
        public string Initials { get; set; }

        [Required]
        public decimal Budget { get; set; }
        
        public int PrimaryKitColorId { get; set; }
        public Color PrimaryKitColor { get; set; }
        
        public int SecondaryKitColorId { get; set; }
        public Color SecondaryKitColor { get; set; }

        public int TownId { get; set; }
        public Town Town { get; set; }

        public ICollection<Player> Players { get; set; }

        [InverseProperty("HomeTeam")]
        public ICollection<Game> HomeGames { get; set; }

        [InverseProperty("AwayTeam")]
        public ICollection<Game> AwayGames { get; set; }
    }
}
