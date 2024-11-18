
namespace App_www_zaliczenie.Models
{
    public class PostNewGameDTO
    {
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        public Category Category { get; set; }
    }
}