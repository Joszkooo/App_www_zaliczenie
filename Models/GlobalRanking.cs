using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace App_www_zaliczenie.Models
{
    public class GlobalRanking
    {
        public int Id { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        
        [ForeignKey("Game")]
        public int GameId { get; set; }

        // Required reference navigation to principal (db relation)
        public Game Game { get; set;} = null!;
    }
}