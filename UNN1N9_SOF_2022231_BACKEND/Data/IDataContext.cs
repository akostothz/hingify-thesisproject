using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using UNN1N9_SOF_2022231_BACKEND.Models;

namespace UNN1N9_SOF_2022231_BACKEND.Data
{
    public interface IDataContext
    {
        public DbSet<Music> Musics { get; set; }
        public DbSet<UserBehavior> UserBehaviors { get; set; }
        public DbSet<LikedSong> LikedSongs { get; set; }
        public DbSet<AppUser> Users { get; set; }

        Task<int> SaveChangesAsync();
        int SaveChanges();

    }

}
