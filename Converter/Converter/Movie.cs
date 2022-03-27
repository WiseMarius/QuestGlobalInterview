using System.Collections.Generic;

namespace Converter
{
    internal class Movie
    {
        public uint Year { get; set; }
        public string Title { get; set; }
        public Info Info { get; set; }
        public string ImageUrl { get; set; }
        public string Plot { get; set; }
        public uint Rank { get; set;}
        public uint RunningTimeSecs{ get; set; }
        public IList<string> Actors { get; set; }

    }
}
