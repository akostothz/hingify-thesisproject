using Microsoft.EntityFrameworkCore;
using UNN1N9_SOF_2022231_BACKEND.Data;
using UNN1N9_SOF_2022231_BACKEND.Models;

namespace UNN1N9_SOF_2022231_BACKEND.Logic
{
    public class MusicLogic : IMusicLogic
    {

        public IEnumerable<Music> GetMusicsByCountry(int id, DataContext _context)
        {        
            var givenuser = _context.Users.FirstOrDefault(x => x.Id == id);
            var usersFromGivenCountry = _context.Users.Where(x => x.Country == givenuser.Country).ToList();
            var behaviours = new List<UserBehavior>();
            var musics = new List<Music>();

            foreach (var behav in _context.UserBehaviors)
            {
                if (ContainsBehaviour(behav.UserId, usersFromGivenCountry))
                {
                    behaviours.Add(behav);
                }
            }
            foreach (var item in _context.Musics)
            {
                if (ContainsMusic(item.Id, behaviours))
                {
                    musics.Add(item);
                }
            }
            return musics;
        }

        private bool ContainsMusic(int id, List<UserBehavior> behaviours)
        {
            foreach (var behaviour in behaviours)
            {
                if (id == behaviour.MusicId)
                {
                    return true;
                }
            }
            return false;
        }

        private bool ContainsBehaviour(int userId, List<AppUser> users)
        {
            foreach (var user in users)
            {
                if (user.Id == userId)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
