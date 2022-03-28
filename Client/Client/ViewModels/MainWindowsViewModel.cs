using Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Client.ViewModels
{
    internal class MainWindowsViewModel : INotifyPropertyChanged
    {
        public List<string> Years { get; set; } = new List<string>();

        #region Movie1
        private Movie _movie1;
        public Movie Movie1
        {
            get { return _movie1; }
            set
            {
                if (_movie1 != value)
                {
                    _movie1 = value;

                    NotifyPropertyChanged(nameof(Movie1));
                }
            }
        }
        #endregion

        #region Movie2
        private Movie _movie2;
        public Movie Movie2
        {
            get { return _movie2; }
            set
            {
                if (_movie2 != value)
                {
                    _movie2 = value;

                    NotifyPropertyChanged(nameof(Movie2));
                }
            }
        }
        #endregion

        #region Movie3
        private Movie _movie3;
        public Movie Movie3
        {
            get { return _movie3; }
            set
            {
                if (_movie3 != value)
                {
                    _movie3 = value;

                    NotifyPropertyChanged(nameof(Movie3));
                }
            }
        }
        #endregion

        #region Movie4
        private Movie _movie4;
        public Movie Movie4
        {
            get { return _movie4; }
            set
            {
                if (_movie4 != value)
                {
                    _movie4 = value;

                    NotifyPropertyChanged(nameof(Movie4));
                }
            }
        }
        #endregion

        #region MoviesFromYear
        private string _moviesFromYear;
        public string MoviesFromYear
        {
            get { return _moviesFromYear; }
            set
            {
                if (_moviesFromYear != value)
                {
                    _moviesFromYear = value;

                    NotifyPropertyChanged(nameof(MoviesFromYear));
                }
            }
        }
        #endregion

        #region SeachBoxValue
        private string _seachBoxValue;
        public string SeachBoxValue
        {
            get { return _seachBoxValue; }
            set
            {
                if (_seachBoxValue != value)
                {
                    _seachBoxValue = value;

                    NotifyPropertyChanged(nameof(SeachBoxValue));

                    if (_seachBoxValue.Length > 2)
                    {
                        SearchMovieFromYearByName(SelectedYear, _seachBoxValue);
                    }
                }
            }
        }
        #endregion

        #region SelectedYear
        private string _selectedYear;
        public string SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                if (_selectedYear != value)
                {
                    _selectedYear = value;

                    NotifyPropertyChanged(nameof(SelectedYear));
                    FetchMoviesFromYear(Int32.Parse(SelectedYear));
                }
            }
        }
        #endregion

        public MainWindowsViewModel()
        {
            const int maxYear = 2016;

            for (int index = 2000; index <= maxYear; ++index)
            {
                Years.Add(index.ToString());
            }

            FetchMoviesFromYear(maxYear);
        }

        private void FetchMoviesFromYear(int year)
        {
            MoviesFromYear = $"Movies from year {year}";

            var result = HttpHelper.Get("http://localhost:2223/api/Movies", new { year = year.ToString() });

            List<Movie> movies = JsonConvert.DeserializeObject<List<Movie>>(result);

            AssignResultedMovies(movies);
        }

        private void SearchMovieFromYearByName(string selectedYear, string seachBoxValue)
        {
            var result = HttpHelper.Get("http://localhost:2223/api/Movies", new { year = selectedYear.ToString(), name = seachBoxValue });

            List<Movie> movies = JsonConvert.DeserializeObject<List<Movie>>(result);

            AssignResultedMovies(movies);
        }

        private void AssignResultedMovies(List<Movie> movies)
        {
            Movie1 = null;
            Movie2 = null;
            Movie3 = null;
            Movie4 = null;

            if (movies.Count > 0)
            {
                ReplaceMissingImageForMovie(movies[0]);
                Movie1 = movies[0];
            }
            if (movies.Count > 1)
            {
                ReplaceMissingImageForMovie(movies[1]);
                Movie2 = movies[1];
            }
            if (movies.Count > 2)
            {
                ReplaceMissingImageForMovie(movies[2]);
                Movie3 = movies[2];
            }
            if (movies.Count > 3)
            {
                ReplaceMissingImageForMovie(movies[3]);
                Movie4 = movies[3];
            }
        }

        private void ReplaceMissingImageForMovie(Movie movie)
        {
            if (string.IsNullOrEmpty(movie.ImageURL))
            {
                movie.ImageURL = "https://media.istockphoto.com/photos/white-puzzle-with-one-missing-piece-picture-id90330304?k=6&m=90330304&s=612x612&w=0&h=oYaXUqIIIojFWmWBg3WapFZ1Xpw6-SVcLIxzb3fk9mo=";
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
