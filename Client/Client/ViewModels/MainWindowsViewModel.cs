using Client.Models;
using System;
using System.Collections.Generic;

namespace Client.ViewModels
{
    internal class MainWindowsViewModel
    {
        public List<string> Years { get; set; } = new List<string>();
        public Movie Movie { get; set; }

        public MainWindowsViewModel()
        {
            for(int index = 2000; index <= 2022; ++index)
            {
                Years.Add(index.ToString());
            }

            Movie = new Movie(1, "testc", 2020, DateTime.Now, 10, "https://ia.media-imdb.com/images/M/MV5BMTQyMDE0MTY0OV5BMl5BanBnXkFtZTcwMjI2OTI0OQ@@._V1_SX400_.jpg", "test plot", 1, 10);
        }

    }
}
