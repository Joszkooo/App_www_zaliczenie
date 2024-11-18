using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace App_www_zaliczenie.Models
{
    public class Account : IdentityUser<int>
    {
        public List<int> VotedGames {get; set;} = new List<int>();
        public UserRanking? UserRanking { get; set; } 
    }
}