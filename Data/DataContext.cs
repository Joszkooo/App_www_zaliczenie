using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace App_www_zaliczenie.Data
{
    public class DataContext : IdentityDbContext<Account, IdentityRole<int>, int>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Game> Games { get; set; }
        public DbSet<GlobalRanking> GlobalRankings { get; set; }
        public DbSet<UserRanking> UserRankings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure custom properties if needed
            builder.Entity<Account>()
                .Property(u => u.VotedGames)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<List<int>>(v, (JsonSerializerOptions)null)
                );
        }
    }
}