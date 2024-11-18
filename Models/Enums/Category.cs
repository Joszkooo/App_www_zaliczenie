using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App_www_zaliczenie.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Category
    {
        RPG = 1,
        FPS = 2,
        Action = 3,
        Adventure = 4,
        Indie = 5
    }
}