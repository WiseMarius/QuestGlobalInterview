using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class MovieDirector
    {
        [Key]
        public int MovieId { get; set; }
        public int DirectorId { get; set; }
    }
}