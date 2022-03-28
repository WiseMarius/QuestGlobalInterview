using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class MoviesController : ApiController
    {
        // GET api/Movies?year=2010
        public IEnumerable<Movie> GetMoviesByYear(int year)
        {
            var results = new List<Movie>();

            using (var context = new Database.Context())
            {
                var movies = context.Movies.SqlQuery("SELECT * FROM Movie WHERE Year == @Year ORDER BY ReleaseDate LIMIT 4", new System.Data.SQLite.SQLiteParameter("@Year", year));

                foreach(var movie in movies)
                {
                    results.Add(movie);
                }
            }

            return results;
        }

        // GET api/Movies?year=2010&name=Ins
        public IEnumerable<Movie> GetMoviesFromYearByName(int year, string name)
        {
            var results = new List<Movie>();

            using (var context = new Database.Context())
            {
                var movies = from movie in context.Movies
                              where movie.Title.Contains(name)
                              where movie.Year == year 
                              select movie;

                foreach (var movie in movies)
                {
                    results.Add(movie);
                }
            }

            return results;
        }
    }
}
