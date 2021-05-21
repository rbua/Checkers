using CheckersGame.JsonModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGame.Models
{
    public class GameModelContext : DbContext
    {
        public GameModelContext(string connString) : base("GameModelContext")
        {
            this.Database.Connection.ConnectionString = connString;
        }
        public DbSet<GameModelStoreModel> GameModels { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
