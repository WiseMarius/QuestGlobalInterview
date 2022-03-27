using System;

namespace WebApplication.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public DateTime ReleaseDate { get; set; }
        public float Rating { get; set; }
        public string ImageURL { get; set; }
        public string Plot { get; set; }
        public int Rank { get; set; }
        public int RunningTimeInSeconds { get; set; }
    }
}