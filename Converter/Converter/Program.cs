using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Converter
{
    internal class Program
    {

        static void Main(string[] args)
        {
            try
            { 
                var movies = ReadMoviesFrom("moviedata.json");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(string.Format("Could not find {0}", ex.FileName));
            }
        }

        private static IList<Movie> ReadMoviesFrom(string fileName)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();

            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName)))
            {
                using (StreamReader streamReader = new StreamReader(fileName))
                {
                    return JsonConvert.DeserializeObject<IList<Movie>>(streamReader.ReadToEnd(), jsonSerializerSettings);
                }
            }
            else
            {
                throw new FileNotFoundException(fileName);
            }
        }
    }
}
