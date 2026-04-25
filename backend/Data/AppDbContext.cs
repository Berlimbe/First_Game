using Microsoft.EntityFrameworkCore;
using First_Game.backend.Domain.Models;

namespace First_Game.backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Definimos quais as tabelas que queremos no banco
        public DbSet<PlayerModel> Players { get; set; }
        public DbSet<RunModel> Runs { get; set; }
        public DbSet<ItemModel> Items { get; set; }
    }
}