using System;

namespace Client.Models
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

        public Movie(int id, string title, int year, DateTime releaseDate, float rating, string imageURL, string plot, int rank, int runningTimeInSeconds)
        {
            Id = id;
            Title = title;
            Year = year;
            ReleaseDate = releaseDate;
            Rating = rating;
            ImageURL = imageURL;
            Plot = plot;
            Rank = rank;
            RunningTimeInSeconds = runningTimeInSeconds;
        }
    }
}
