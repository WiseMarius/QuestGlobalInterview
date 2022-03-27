using System;
using System.Data.SQLite;
using System.Linq;

namespace Converter
{
    internal class Commands
    {
        public static SQLiteCommand InsertMovieCommand(Movie movie, SQLiteConnection connection)
        {
            string commandString = $@"INSERT INTO Movie (
                      RunningTimeInSeconds,
                      Rank,
                      Plot,
                      ImageURL,
                      Rating,
                      ReleaseDate,
                      Title,
                      Year
                  )
                  VALUES (
                      @RunningTimeInSeconds,
                      @Rank,
                      @Plot,
                      @ImageURL,
                      @Rating,
                      @ReleaseDate,
                      @Title,
                      @Year
                  );".Replace(Environment.NewLine, " ");

            var command = new SQLiteCommand(commandString, connection);
            command.Parameters.Add(new SQLiteParameter("@RunningTimeInSeconds",  movie.Info.RunningTimeSecs));
            command.Parameters.Add(new SQLiteParameter("@Rank",  movie.Info.Rank));
            command.Parameters.Add(new SQLiteParameter("@Plot",  movie.Info.Plot));
            command.Parameters.Add(new SQLiteParameter("@ImageURL",  movie.Info.ImageUrl));
            command.Parameters.Add(new SQLiteParameter("@Rating",  movie.Info.Rating));
            command.Parameters.Add(new SQLiteParameter("@ReleaseDate",  movie.Info.ReleaseDate));
            command.Parameters.Add(new SQLiteParameter("@Title",  movie.Title));
            command.Parameters.Add(new SQLiteParameter("@Year",  movie.Year));

            return command;
        }

        internal static SQLiteCommand InsertEntryWithNameInTableCommand(string entry, string table, SQLiteConnection connection)
        {
            CheckForWhiteSpaces(table);

            string commandString = $@"INSERT INTO {table} (Name) VALUES (@Name)";
            var command = new SQLiteCommand(commandString, connection);

            command.Parameters.Add(new SQLiteParameter("@Name", entry));

            return command;
        }

        internal static SQLiteCommand GetIdByNameInTableCommand(string entry, string table, SQLiteConnection connection)
        {
            CheckForWhiteSpaces(table);

            var commandString = $@"SELECT Id FROM {table} WHERE Name = @Name";
            var command = new SQLiteCommand(commandString, connection);
            command.Parameters.Add(new SQLiteParameter("@Name", entry));

            return command;
        }

        internal static SQLiteCommand GetIdByMovieTitleCommand(string title, SQLiteConnection connection)
        {
            var commandString = $@"SELECT Id FROM Movie WHERE Title = @Title";
            var command = new SQLiteCommand(commandString, connection);
            command.Parameters.Add(new SQLiteParameter("@Title", title));

            return command;
        }

        internal static SQLiteCommand InsertRelationBetweenMovieAnd(string relationTableName, string relationTableIdName, int movieId, int otherId, SQLiteConnection connection)
        {
            CheckForWhiteSpaces(relationTableName);
            CheckForWhiteSpaces(relationTableIdName);

            string commandString = $@"INSERT INTO {relationTableName} (MovieId, {relationTableIdName}) VALUES (@MovieId, @OtherId)";
            var command = new SQLiteCommand(commandString, connection);

            command.Parameters.Add(new SQLiteParameter("@MovieId", movieId.ToString()));
            command.Parameters.Add(new SQLiteParameter("@OtherId", otherId.ToString()));

            return command;
        }

        internal static void CheckForWhiteSpaces(string table)
        {
            if (table.Any(x => Char.IsWhiteSpace(x)))
            {
                throw new Exception("Whitespace not allowed in order to avoid SQL Injection");
            }
        }
    }
}