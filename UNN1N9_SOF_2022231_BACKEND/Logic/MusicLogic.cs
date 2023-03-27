using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UNN1N9_SOF_2022231_BACKEND.Data;
using UNN1N9_SOF_2022231_BACKEND.DTOs;
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

        public void AddLikedSong(int userid, int musicid)
        {
            var givenuser = _context.Users.FirstOrDefault(x => x.Id == userid);
            string nameOfDay = DateTime.Now.DayOfWeek.ToString();
            string timeOfDay = TimeOfDayConverter();

            _context.UserBehaviors.Add(new UserBehavior()
            {
                UserId = userid,
                MusicId = musicid,
                ListeningCount = 1,
                NameOfDay = nameOfDay,
                TimeOfDay = timeOfDay
            });

            _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<string>> GetStyles(int id)
        {
            var givenuser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
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

            foreach (var music in musics)
            {
                if (!genres.Contains(music.Genre))
                {
                    genres.Add(music.Genre);
                }
            }

            genres.Add("Mixed");

            return genres;
        }

        public async Task<IEnumerable<Music>> GetPersonalizedMix(int id)
        {
            var givenuser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            string nameOfDay = DateTime.Now.DayOfWeek.ToString();
            string timeOfDay = TimeOfDayConverter();
            var musics = new List<Music>();
            var behaviours = new List<UserBehavior>();
            var genres = new List<string>();
            var style = "";

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

            // stílusok számának lekérdezése; ha több, mint 5, akkor vegyes mixet kap, ha nem, akkor azt amiből a legtöbb van

            var dict = new Dictionary<string, int>();

            foreach (var music in musics)
            {
                if (!genres.Contains(music.Genre))
                {
                    genres.Add(music.Genre);
                    dict[music.Genre] = 1;
                }
                else
                {
                    dict[music.Genre] += 1;
                }
            }

            if (genres.Count >= 5) //vegyes mix
            {

            }
            else //mix a legtöbbet előfordultból
            {
                style = dict.Aggregate((x, y) => x.Value > y.Value ? x : y).Key; //a stílus neve
            }

            //a napszakból való hátramaradó idő kiszámítása
            int endHour = EndHour(timeOfDay);
            int currHour = DateTime.UtcNow.Hour;
            int currMins = DateTime.UtcNow.Minute;

            int hoursLeft = 0;
            int minsLeft = 0;

            if (currHour != endHour)
                hoursLeft = endHour - currHour;

            minsLeft = 60 - currMins;

            if (hoursLeft != 0)
                minsLeft = minsLeft + hoursLeft * 60;

            //itt ki van számítva, minsLeft hosszúságú lista kell vissza

            var selectedMusics = new List<Music>();

            var songsToChooseFrom = new List<Music>();

            //kevert megoldás


            //nem kevert megoldás

            foreach (var music in musics)
            {
                foreach (var allM in _context.Musics.Where(x => x.Genre == style && x.Mode == music.Mode))
                {
                    if (EnergyInBourdaries(music.Energy, allM.Energy)
                        && ValenceInBourdaries(music.Valence, allM.Valence) &&
                        AcousticnessInBourdaries(music.Acousticness, allM.Acousticness))
                    {
                        songsToChooseFrom.Add(allM);
                    }
                }
            }

            //le van szűrve az összes választható szám - márcsak végig kell menni,
            //euklidészi távolságot számolni, prioritásos sorba rakni és a kimeneti zenékbe rakni amíg belefér

            foreach (var music in musics)
            {

            }


            return selectedMusics;
        }

        private double EuclideanDistance(Music currMusic, Music dbMusic)
        {
            double dist = Math.Sqrt(Math.Pow(currMusic.Energy - dbMusic.Energy, 2) +
                                Math.Pow(currMusic.Valence - dbMusic.Valence, 2) +
                                Math.Pow(currMusic.Acousticness - dbMusic.Acousticness, 2));
            return dist;
        }

        public async Task<IEnumerable<Music>> GetPersonalizedMix2(int id, string style)
        {
            var givenuser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
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
            if (style.ToLower() == "mixed")
            {
                foreach (var music in musics)
                {
                    if (!genres.Contains(music.Genre))
                    {
                        genres.Add(music.Genre);
                    }
                }
            }
            else
            {
                foreach (var music in musics)
                {
                    if (music.Genre == style)
                    {
                        genres.Add(music.Genre);
                    }
                }
            }

            //itt megvannak a hallgatott stílusok abban az időszakban, ahol épp vagyunk

            //a napszakból való hátramaradó idő kiszámítása
            int endHour = EndHour(timeOfDay);
            int currHour = DateTime.UtcNow.Hour;
            int currMins = DateTime.UtcNow.Minute;

            int hoursLeft = 0;
            int minsLeft = 0;

            if (currHour != endHour)
                hoursLeft = endHour - currHour;

            minsLeft = 60 - currMins;

            if (hoursLeft != 0)
                minsLeft = minsLeft + hoursLeft * 60;

            //itt ki van számítva, minsLeft hosszúságú lista kell vissza

            var selectedMusics = new List<Music>();

            //először egy szűrés 0.05 távolságon belül

            var songsToChooseFrom = new List<Music>();
            var currQuery = new List<Music>();

            //ha kevertet kér


            //ha nemn

            foreach (var music in musics)
            {
                foreach (var allM in _context.Musics.Where(x => x.Genre == style && x.Mode == music.Mode))
                {
                    if (EnergyInBourdaries(music.Energy, allM.Energy) 
                        && ValenceInBourdaries(music.Valence, allM.Valence) && 
                        AcousticnessInBourdaries(music.Acousticness, allM.Acousticness))
                    {
                        songsToChooseFrom.Add(allM);
                    }
                }

                /*
                 currQuery = from x in _context.Musics
                            where x.Genre == music.Genre
                            && x.Mode == music.Mode
                            && x.Energy <= music.Energy + 0.05
                            && x.Energy >= music.Energy - 0.05
                            && x.Valence <= music.Valence + 0.05
                            && x.Valence >= music.Valence - 0.05
                            && x.Acousticness <= music.Acousticness + 0.05
                            && x.Acousticness >= music.Acousticness - 0.05
                            select x;
                 */
            }

            //minden zenéhez az euklidészi távolság



            return selectedMusics;
        }

        private bool EnergyInBourdaries(double currEnergy, double dbEnergy)
        {
            if (dbEnergy <= currEnergy + 0.05 && dbEnergy >= currEnergy - 0.05)
                return true;

            return false;
        }

        private bool ValenceInBourdaries(double currValence, double dbValence)
        {
            if (dbValence <= currValence + 0.05 && dbValence >= currValence - 0.05)
                return true;

            return false;
        }

        private bool AcousticnessInBourdaries(double currAcousticness, double dbAcousticness)
        {
            if (dbAcousticness <= currAcousticness + 0.05 && dbAcousticness >= currAcousticness - 0.05)
                return true;

            return false;
        }

        private int EndHour(string timeOfDay)
        {
            if (timeOfDay.ToLower() == "dawn")
                return 5;
            else if (timeOfDay.ToLower() == "morning")
                return 9;
            else if (timeOfDay.ToLower() == "forenoon")
                return 12;
            else if (timeOfDay.ToLower() == "afternoon")
                return 17;
            else if (timeOfDay.ToLower() == "evening")
                return 21;
            else
                return 24;
        }

        public async Task<IEnumerable<Music>> GetLikedSongs(int id)
        {
            var givenuser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
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

        public async Task<IEnumerable<Music>> GetMusicsBySex(int id)
        {
            var givenuser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
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

        public async Task<IEnumerable<Music>> GetMusicsByCountry(int id)
        {
            var givenuser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
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

        public async Task<IEnumerable<Music>> GetMusicsByAgeGroup(int id)
        {
            int lowerLimit = 0;
            int upperLimit = 0;
            var givenuser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            var musics = new List<Music>();
            var behaviours = new List<UserBehavior>();
            var usersFromGivenAgeGroup = new List<AppUser>();

            int age = AgeCalculator(givenuser.YearOfBirth);
            AgeGroupSetter(ref lowerLimit, ref upperLimit, age);

            foreach (var user in _context.Users)
            {
                if (AgeCalculator(user.YearOfBirth) >= lowerLimit && AgeCalculator(user.YearOfBirth) <= upperLimit)
                {
                    usersFromGivenAgeGroup.Add(user);
                }
            }

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

        public string TimeOfDayConverter()
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

        private int AgeCalculator(int yofBirth)
        {
            return (int)DateTime.Now.Year - yofBirth;
        }

        
    }
}
