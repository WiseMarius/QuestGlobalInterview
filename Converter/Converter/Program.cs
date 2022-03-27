using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SQLite;
using System.Linq;

namespace Converter
{
    internal class Program
    {
        private const string DirectorTableName = "Director";
        private const string GenreTableName = "Genre";
        private const string ActorTableName = "Actor";
        private const string MovieDirectorTableName = "MovieDirector";
        private const string MovieGenreTableName = "MovieGenre";
        private const string MovieActorTableName = "MovieActor";

        static void Main(string[] args)
        {
            try
            {
                var movies = ReadMoviesFrom("moviedata.json");
                WriteMoviesTo(movies, "moviedata.sqlite");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(string.Format("Could not find {0}", ex.FileName));
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
            Console.WriteLine("Nice");
        }

        private static IList<Movie> ReadMoviesFrom(string fileName)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();

            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName)))
            {
                using (StreamReader streamReader = new StreamReader(fileName))
                {
                    return JsonConvert.DeserializeObject<List<Movie>>(streamReader.ReadToEnd(), jsonSerializerSettings);
                }
            }
            else
            {
                throw new FileNotFoundException(fileName);
            }
        }

        private static void WriteMoviesTo(IList<Movie> movies, string databaseFileName)
        {
            try
            {
                SQLiteConnection.CreateFile(databaseFileName);
            }
            catch (IOException)
            {
                throw new IOException($"{databaseFileName} already in use");
            }

            using (var connection = new SQLiteConnection($"Data Source={databaseFileName};"))
            {
                connection.Open();

                AddDatabaseSchemaTo(connection);
                AddMoviesTo(movies, connection);
            }
        }
        private static void AddDatabaseSchemaTo(SQLiteConnection connection)
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                var createDirectorSchemaCommand = new SQLiteCommand(DatabaseSchema.DirectorSchema, connection);
                createDirectorSchemaCommand.ExecuteNonQuery();

                var createGenreSchemaCommand = new SQLiteCommand(DatabaseSchema.GenreSchema, connection);
                createGenreSchemaCommand.ExecuteNonQuery();

                var createActorSchemaCommand = new SQLiteCommand(DatabaseSchema.ActorSchema, connection);
                createActorSchemaCommand.ExecuteNonQuery();

                var createMovieSchemaCommand = new SQLiteCommand(DatabaseSchema.MovieSchema, connection);
                createMovieSchemaCommand.ExecuteNonQuery();

                var createMovieDirectorSchemaCommand = new SQLiteCommand(DatabaseSchema.MovieDirectorSchema, connection);
                createMovieDirectorSchemaCommand.ExecuteNonQuery();

                var createMovieGenreSchemaCommand = new SQLiteCommand(DatabaseSchema.MovieGenreSchema, connection);
                createMovieGenreSchemaCommand.ExecuteNonQuery();

                var createMovieActorSchemaCommand = new SQLiteCommand(DatabaseSchema.MovieActorSchema, connection);
                createMovieActorSchemaCommand.ExecuteNonQuery();
            }
        }

        private static void AddMoviesTo(IList<Movie> movies, SQLiteConnection connection)
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                Console.Write("Creating database... ");

                using (var progress = new ProgressBar())
                {
                    for (var index = 0; index < movies.Count; ++index)
                    {
                        using (var insertCommand = Commands.InsertMovieCommand(movies[index], connection))
                        {
                            insertCommand.ExecuteNonQuery();
                        }

                        AddDirectorIfMissing(movies[index], connection);
                        AddGenreIfMissing(movies[index], connection);
                        AddActorIfMissing(movies[index], connection);

                        CreateRelationBetweenMovieAndDirectors(movies[index], connection);
                        CreateRelationBetweenMovieAndGenres(movies[index], connection);
                        CreateRelationBetweenMovieAndActors(movies[index], connection);

                        progress.Report((double)index / movies.Count);
                    }
                }
            }
        }

        private static void AddActorIfMissing(Movie movie, SQLiteConnection connection)
        {
            foreach (var actor in movie.Info.Actors)
            {
                if (!EntryExistsInTable(actor, ActorTableName, connection))
                {
                    Commands.InsertEntryWithNameInTableCommand(actor, ActorTableName, connection).ExecuteNonQuery();
                }
            }
        }

        private static void AddGenreIfMissing(Movie movie, SQLiteConnection connection)
        {
            foreach (var genre in movie.Info.Genres)
            {
                if (!EntryExistsInTable(genre, GenreTableName, connection))
                {
                    Commands.InsertEntryWithNameInTableCommand(genre, GenreTableName, connection).ExecuteNonQuery();
                }
            }
        }

        private static void AddDirectorIfMissing(Movie movie, SQLiteConnection connection)
        {
            foreach (var director in movie.Info.Directors)
            {
                if (!EntryExistsInTable(director, DirectorTableName, connection))
                {
                    Commands.InsertEntryWithNameInTableCommand(director, DirectorTableName, connection).ExecuteNonQuery();
                }
            }
        }

        private static bool EntryExistsInTable(string entry, string tableName, SQLiteConnection connection)
        {
            using (var entryExistsCommand = Commands.GetIdByNameInTableCommand(entry, tableName, connection))
            {
                return entryExistsCommand.ExecuteReader().Read();
            }
        }

        private static void CreateRelationBetweenMovieAndDirectors(Movie movie, SQLiteConnection connection)
        {
            var movieId = GetMovieId(movie.Title, connection);

            foreach (var director in movie.Info.Directors)
            {
                var directorId = GetEntryIdFromTable(director, DirectorTableName, connection);

                Commands.InsertRelationBetweenMovieAnd(MovieDirectorTableName, "DirectorId", movieId, directorId, connection).ExecuteNonQuery();
            }
        }

        private static void CreateRelationBetweenMovieAndGenres(Movie movie, SQLiteConnection connection)
        {
            var movieId = GetMovieId(movie.Title, connection);

            foreach (var genre in movie.Info.Genres)
            {
                var genreId = GetEntryIdFromTable(genre, GenreTableName, connection);

                Commands.InsertRelationBetweenMovieAnd(MovieGenreTableName, "GenreId", movieId, genreId, connection).ExecuteNonQuery();
            }
        }

        private static void CreateRelationBetweenMovieAndActors(Movie movie, SQLiteConnection connection)
        {
            var movieId = GetMovieId(movie.Title, connection);

            foreach (var actor in movie.Info.Actors)
            {
                var actorId = GetEntryIdFromTable(actor, ActorTableName, connection);

                Commands.InsertRelationBetweenMovieAnd(MovieActorTableName, "ActorId", movieId, actorId, connection).ExecuteNonQuery();
            }
        }

        private static int GetEntryIdFromTable(string entry, string table, SQLiteConnection connection)
        {
            int id;

            using (var entryExistsCommand = Commands.GetIdByNameInTableCommand(entry, table, connection))
            using (var reader = entryExistsCommand.ExecuteReader())
            {
                reader.Read();

                id = reader.GetInt32(0);
            }

            return id;
        }

        private static int GetMovieId(string entry, SQLiteConnection connection)
        {
            int id;

            using (var entryExistsCommand = Commands.GetIdByMovieTitleCommand(entry, connection))
            using (var reader = entryExistsCommand.ExecuteReader())
            {
                reader.Read();

                id = reader.GetInt32(0);
            }

            return id;
        }
    }
}
