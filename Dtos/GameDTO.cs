using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App_www_zaliczenie.Dtos
{
    public class GameDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
    }
}