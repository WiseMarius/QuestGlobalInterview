using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class MovieActor
    {
        [Key]
        public int MovieId { get; set; }
        public int ActorId { get; set; }
    }
}