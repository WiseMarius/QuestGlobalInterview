using System;
using System.Collections.Generic;
using System.Data.SQLite;

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

        internal static SQLiteCommand InsertDirectorCommand(string directorName, SQLiteConnection connection)
        {
            string commandString = $@"INSERT INTO Director (Name) VALUES (@Name)";
            var command = new SQLiteCommand(commandString, connection);

            command.Parameters.Add(new SQLiteParameter("@Name", directorName));

            return command;
        }

        internal static SQLiteCommand DirectorExistsCommand(string directorName, SQLiteConnection connection)
        {
            var commandString = $@"SELECT Id FROM Director WHERE Name = @Name";
            var command = new SQLiteCommand(commandString, connection);
            command.Parameters.Add(new SQLiteParameter("@Name", directorName));

            return command;
        }
    }
}