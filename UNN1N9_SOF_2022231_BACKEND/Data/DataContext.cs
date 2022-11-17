using Microsoft.EntityFrameworkCore;
using UNN1N9_SOF_2022231_BACKEND.Models;

namespace UNN1N9_SOF_2022231_BACKEND.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Music> Musics { get; set; }
        public DbSet<UserBehavior> UserBehaviors { get; set; }


    }
}
