using System;
using System.Collections.Generic;

namespace Converter
{
    internal class Info
    {
        public IList<string> Directors { get; set; }
        public DateTime ReleaseDate { get; set; }
        public float Rating { get; set; }
        public IList<string> Genres { get; set; }
    }
}
