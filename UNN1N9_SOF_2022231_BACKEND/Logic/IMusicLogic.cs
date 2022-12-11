using UNN1N9_SOF_2022231_BACKEND.Data;
using UNN1N9_SOF_2022231_BACKEND.Models;

namespace UNN1N9_SOF_2022231_BACKEND.Logic
{
    public interface IMusicLogic
    {
        IEnumerable<Music> GetPersonalizedMix(int id);
        IEnumerable<Music> GetLikedSongs(int id);
        IEnumerable<Music> GetMusicsBySex(int id);
        IEnumerable<Music> GetMusicsByCountry(int id);
        IEnumerable<Music> GetMusicsByAgeGroup(int id);
    }
}
