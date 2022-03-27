using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite;
using WebApplication.Models;

namespace WebApplication.Database
{
    public class Context : DbContext
    {
        public Context() : base(new SQLiteConnection()
                            {
                                ConnectionString = new SQLiteConnectionStringBuilder()
                                {
                                    // IMPORTANT! moviedata.sqlite needs to be placed here C:\Program Files\IIS Express 
                                    DataSource = "moviedata.sqlite",
                                    ForeignKeys = true,
                                }.ConnectionString
                            }, true)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Director> Director { get; set;  }
    }
}