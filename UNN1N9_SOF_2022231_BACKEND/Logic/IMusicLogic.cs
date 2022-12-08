using UNN1N9_SOF_2022231_BACKEND.Data;
using UNN1N9_SOF_2022231_BACKEND.Models;

namespace UNN1N9_SOF_2022231_BACKEND.Logic
{
    public interface IMusicLogic
    {
        IEnumerable<Music> GetMusicsByCountry(int id, DataContext _context);
    }
}
