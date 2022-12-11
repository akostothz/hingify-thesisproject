using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UNN1N9_SOF_2022231_BACKEND.Data;
using UNN1N9_SOF_2022231_BACKEND.Models;

namespace UNN1N9_SOF_2022231_BACKEND.Logic
{
    public class MusicLogic : IMusicLogic
    {
        DataContext _context;

        public MusicLogic(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<Music> GetPersonalizedMix(int id)
        {
            var givenuser = _context.Users.FirstOrDefault(x => x.Id == id);
            string nameOfDay = DateTime.Now.DayOfWeek.ToString();
            string timeOfDay = TimeOfDayConverter();
            var musics = new List<Music>();
            var behaviours = new List<UserBehavior>();
            var genres = new List<string>();

            foreach (var behav in _context.UserBehaviors)
            {
                if (behav.UserId == givenuser.Id && behav.NameOfDay.Equals(nameOfDay) && behav.TimeOfDay.Equals(timeOfDay))
                {
                    behaviours.Add(behav);
                }
            }
            foreach (var music in _context.Musics)
            {
                if (ContainsMusic(music.Id, behaviours))
                {
                    musics.Add(music);
                }
            }
            //itt megvannak a hallgatott zenék abban az időszakban, ahol épp vagyunk
            foreach (var music in musics)
            {
                if (!genres.Contains(music.Genre))
                {
                    genres.Add(music.Genre);
                }
            }
            //itt megvannak a hallgatott stílusok abban az időszakban, ahol épp vagyunk

            //itt kéne maga a Cluster kialakítása, és a top x zenét visszaadni visszaadni
            var selectedMusics = new List<Music>();


            return selectedMusics;
        }

        public IEnumerable<Music> GetLikedSongs(int id)
        {
            var givenuser = _context.Users.FirstOrDefault(x => x.Id == id);
            var musics = new List<Music>();
            var behaviours = new List<UserBehavior>();
            foreach (var behav in _context.UserBehaviors)
            {
                if (id == behav.UserId)
                {
                    behaviours.Add(behav);
                }
            }
            foreach (var music in _context.Musics)
            {
                if (ContainsMusic(music.Id, behaviours))
                {
                    musics.Add(music);
                }
            }
            return musics;
        }

        public IEnumerable<Music> GetMusicsBySex(int id)
        {
            var givenuser = _context.Users.FirstOrDefault(x => x.Id == id);
            var usersFromGivenSex = _context.Users.Where(x => x.Gender == givenuser.Gender).ToList();
            var behaviours = new List<UserBehavior>();
            var musics = new List<Music>();

            foreach (var behav in _context.UserBehaviors)
            {
                if (ContainsIt(behav.UserId, usersFromGivenSex))
                {
                    behaviours.Add(behav);
                }
            }
            foreach (var music in _context.Musics)
            {
                if (ContainsMusic(music.Id, behaviours))
                {
                    musics.Add(music);
                }
            }
            return musics;
        }

        public IEnumerable<Music> GetMusicsByCountry(int id)
        {
            var givenuser = _context.Users.FirstOrDefault(x => x.Id == id);
            var usersFromGivenCountry = _context.Users.Where(x => x.Country == givenuser.Country).ToList();
            var behaviours = new List<UserBehavior>();
            var musics = new List<Music>();

            foreach (var behav in _context.UserBehaviors)
            {
                if (ContainsIt(behav.UserId, usersFromGivenCountry))
                {
                    behaviours.Add(behav);
                }
            }
            foreach (var music in _context.Musics)
            {
                if (ContainsMusic(music.Id, behaviours))
                {
                    musics.Add(music);
                }
            }
            return musics;
        }

        public IEnumerable<Music> GetMusicsByAgeGroup(int id)
        {
            int lowerLimit = 0;
            int upperLimit = 0;
            var givenuser = _context.Users.FirstOrDefault(x => x.Id == id);
            var musics = new List<Music>();
            var behaviours = new List<UserBehavior>();
            //int age = AgeChechker(DateTime);      ez majd ha átálltam DateTime használatára
            AgeGroupSetter(ref lowerLimit, ref upperLimit, givenuser.Age);
            var usersFromGivenAgeGroup = _context.Users.Where(x => x.Age >= lowerLimit && x.Age <= upperLimit).ToList();

            foreach (var behav in _context.UserBehaviors)
            {
                if (ContainsIt(behav.UserId, usersFromGivenAgeGroup))
                {
                    behaviours.Add(behav);
                }
            }
            foreach (var music in _context.Musics)
            {
                if (ContainsMusic(music.Id, behaviours))
                {
                    musics.Add(music);
                }
            }

            return musics;
        }

        private string TimeOfDayConverter()
        {
            int time = DateTime.Now.Hour;

            if (time < 5)
                return "Dawn";
            else if (time >= 5 && time < 9)
                return "Morning";
            else if (time >= 9 && time < 12)
                return "Forenoon";
            else if (time >= 12 && time < 17)
                return "Afternoon";
            else if (time >= 17 && time < 21)
                return "Evening";
            else
                return "Night";
        }

        private bool ContainsIt(int userId, List<AppUser> users)
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
        private void AgeGroupSetter(ref int lowerLimit, ref int upperLimit, int age)
        {
            if (age <= 11)
            {
                lowerLimit = 1;
                upperLimit = 11;
            }
            if (age <= 17 && age > 11)
            {
                lowerLimit = 12;
                upperLimit = 17;
            }
            if (age <= 25 && age > 17)
            {
                lowerLimit = 18;
                upperLimit = 25;
            }
            if (age <= 39 && age > 25)
            {
                lowerLimit = 26;
                upperLimit = 39;
            }
            if (age <= 59 && age > 39)
            {
                lowerLimit = 40;
                upperLimit = 59;
            }
            if (age >= 60)
            {
                lowerLimit = 60;
                upperLimit = 120; //max age per se
            }
        }

        private int AgeChechker(DateTime dateTime)
        {
            throw new NotImplementedException();
        }
    }
}
