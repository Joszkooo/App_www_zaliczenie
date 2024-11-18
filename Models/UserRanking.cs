using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace App_www_zaliczenie.Models
{
    public class UserRanking
    {
        public int Id { get; set; }
        public bool UpVote { get; set; }
        public bool DownVote { get; set; }
        
        [ForeignKey("Game")]
        public int GameId { get; set; }
        
        [ForeignKey("Account")]
        public int AccountId { get; set; }


        // Required reference navigation to principal (db relation)
        public Game Game { get; set;} = null!;
        public Account Account {get; set; } = null!;    
    }
}