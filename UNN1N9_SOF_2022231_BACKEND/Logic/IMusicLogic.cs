using UNN1N9_SOF_2022231_BACKEND.Data;
using UNN1N9_SOF_2022231_BACKEND.Models;

namespace UNN1N9_SOF_2022231_BACKEND.Logic
{
    public interface IMusicLogic
    {
        Task<IEnumerable<string>> GetStyles(int id);
        Task<IEnumerable<Music>> GetPersonalizedMix(int id);
        Task<IEnumerable<Music>> GetLikedSongs(int id);
        Task<IEnumerable<Music>> GetMusicsBySex(int id);
        Task<IEnumerable<Music>> GetMusicsByCountry(int id);
        Task<IEnumerable<Music>> GetMusicsByAgeGroup(int id);
        void AddLikedSong(int userid, int musicid);
        public string TimeOfDayConverter();
    }
}
