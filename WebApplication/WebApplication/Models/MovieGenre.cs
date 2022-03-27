using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class MovieGenre
    {
        [Key]
        public int MovieId { get; set; }
        public int GenreId { get; set; }
    }
}