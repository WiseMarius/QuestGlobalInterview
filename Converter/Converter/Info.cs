using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Converter
{
    internal class Info
    {
        [JsonProperty(PropertyName = "directors")]
        public List<string> Directors { get; set; } = new List<string>();

        [JsonProperty(PropertyName = "release_date")]
        public DateTime ReleaseDate { get; set; }

        [JsonProperty(PropertyName = "rating")]
        public float Rating { get; set; }

        [JsonProperty(PropertyName = "genres")]
        public List<string> Genres { get; set; } = new List<string>();

        [JsonProperty(PropertyName = "image_url")]
        public string ImageUrl { get; set; }

        [JsonProperty(PropertyName = "plot")]
        public string Plot { get; set; }

        [JsonProperty(PropertyName = "rank")]
        public uint Rank { get; set; }

        [JsonProperty(PropertyName = "running_time_secs")]
        public uint RunningTimeSecs { get; set; }

        [JsonProperty(PropertyName = "actors")]
        public List<string> Actors { get; set; } = new List<string>();
    }
}
