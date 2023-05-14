using UNN1N9_SOF_2022231_BACKEND.Data;
using UNN1N9_SOF_2022231_BACKEND.DTOs;
using UNN1N9_SOF_2022231_BACKEND.Models;

namespace UNN1N9_SOF_2022231_BACKEND.Logic
{
    public interface IMusicLogic
    {
        Task<IEnumerable<string>> GetStyles(int id);
        Task<IEnumerable<Music>> GetPersonalizedMix(int id);
        Task<IEnumerable<Music>> GetActualLikedSongs(int id);
        Task<IEnumerable<Music>> GetLikedSongs(int id);
        Task<IEnumerable<Music>> GetMusicsBySex(int id);
        Task<IEnumerable<Music>> GetMusicsByCountry(int id);
        Task<IEnumerable<Music>> GetMusicsByAgeGroup(int id);
        Task<IEnumerable<Music>> FindMusic(string trackId);
        Task<IEnumerable<Music>> FindMore(string trackId);
        Task<IEnumerable<Music>> Search(string expr);
        Task<IEnumerable<Music>> FindMoreByArtist(string expr);
        Task<IEnumerable<StatDto>> GetDailyStatistics(int id);
        Task<IEnumerable<StatDto>> GetWeeklyStatistics(int id);
        Task<IEnumerable<StatDto>> GetMonthlyStatistics(int id);
        Task<IEnumerable<StatDto>> GetYearlyStatistics(int id);
        Task<AppUser> GetUser(int id);
        void RetrieveAccessToken(AccessTokenDTO accessToken);
        void RefreshToken(AccessTokenDTO accessToken);
        bool IsLiked(int userid, int musicid);
        void AddLikedSong(int userid, int musicid);
        void RemoveFromLikedSong(int userid, int musicid);
        public string TimeOfDayConverter();
    }
}
