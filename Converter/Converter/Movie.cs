using Newtonsoft.Json;
using System.Collections.Generic;

namespace Converter
{
    internal class Movie
    {
        [JsonProperty(PropertyName = "year")]
        public uint Year { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "info")]
        public Info Info { get; set; }
    }
}
