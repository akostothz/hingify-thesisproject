using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using UNN1N9_SOF_2022231_BACKEND.Models;

namespace UNN1N9_SOF_2022231_BACKEND.Data
{
    public class DataContext : DbContext, IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return base.SaveChanges();
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Music> Musics { get; set; }
        public DbSet<UserBehavior> UserBehaviors { get; set; }
        public DbSet<LikedSong> LikedSongs { get; set; }

    }
}
