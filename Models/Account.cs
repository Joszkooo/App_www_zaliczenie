using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App_www_zaliczenie.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Salt { get; set; }
    }
}