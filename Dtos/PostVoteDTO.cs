using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace App_www_zaliczenie.Dtos
{
    public class PostVoteDTO
    {
        public string Name { get; set; } = string.Empty;
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
    }
}