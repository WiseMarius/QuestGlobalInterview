namespace Converter
{
    internal class DatabaseSchema
    {
        public const string DirectorSchema = @"CREATE TABLE Director (
                                                Id INTEGER PRIMARY KEY ASC AUTOINCREMENT
                                                             UNIQUE
                                                             NOT NULL,
                                                Name TEXT    NOT NULL);";

        public const string GenreSchema = @"CREATE TABLE Genre (
                                                Id INTEGER PRIMARY KEY ASC AUTOINCREMENT
                                                             UNIQUE
                                                             NOT NULL,
                                                Name TEXT    NOT NULL);";

        public const string ActorSchema = @"CREATE TABLE Actor (
                                                Id INTEGER PRIMARY KEY ASC AUTOINCREMENT
                                                             UNIQUE
                                                             NOT NULL,
                                                Name TEXT    NOT NULL);";

        public const string MovieSchema = @"CREATE TABLE Movie (
                                                Id                   INTEGER  PRIMARY KEY ASC AUTOINCREMENT
                                                                              UNIQUE
                                                                              NOT NULL,
                                                Year                 INTEGER  NOT NULL,
                                                Title                TEXT     NOT NULL,
                                                ReleaseDate          DATETIME NOT NULL,
                                                Rating               REAL     NOT NULL,
                                                ImageURL             TEXT     ,
                                                Plot                 TEXT     ,
                                                Rank                 INT      UNIQUE
                                                                              NOT NULL,
                                                RunningTimeInSeconds INT      NOT NULL);";

        public const string MovieDirectorSchema = @"CREATE TABLE MovieDirector (
                                                MovieId    INTEGER REFERENCES Movie (Id) 
                                                                   NOT NULL,
                                                DirectorId INTEGER REFERENCES Director (Id) 
                                                                   NOT NULL
                                            );";

        public const string MovieGenreSchema = @"CREATE TABLE MovieGenre (
                                                MovieId INTEGER NOT NULL
                                                                REFERENCES Movie (Id),
                                                GenreId INTEGER NOT NULL
                                                                REFERENCES Genre (Id) 
                                            );";

        public const string MovieActorSchema = @"CREATE TABLE MovieActor (
                                                MovieId INTEGER NOT NULL
                                                                REFERENCES Movie (Id),
                                                ActorId INTEGER NOT NULL
                                                                REFERENCES ActorId (Id) 
                                            );";
    }
}
