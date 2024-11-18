using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace App_www_zaliczenie.Models
{
    public class Game
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        [Required]
        public Category Category { get; set; }

        // Reference navigation (for db foregin key)
        public GlobalRanking? GlobalRanking { get; set; } 
        public UserRanking? UserRanking { get; set; }
    }
}