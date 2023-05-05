using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotifyWebApi;
using System.Collections.Generic;
using UNN1N9_SOF_2022231_BACKEND.Data;
using UNN1N9_SOF_2022231_BACKEND.DTOs;
using UNN1N9_SOF_2022231_BACKEND.Helpers;
using UNN1N9_SOF_2022231_BACKEND.Models;
using static SpotifyAPI.Web.PlaylistRemoveItemsRequest;

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
            //euklidészi távolságot számolni, prioritásos sorba rakni és a kimeneti zenékbe rakni amíg belefér -> lehetne láncolt lista is, ahol távolság alapján szúrjuk be

            //PriorityQueue<(double, Music)> closestObjects = new PriorityQueue<(double, Music)>();
            LinkedList closestMusics = new LinkedList();

            foreach (var music in musics) //végigmegyünk a zenéken
            {
                foreach (var item in songsToChooseFrom) //és a választható zenéken
                {
                    while (EnergyInBourdaries(music.Energy, item.Energy)
                        && ValenceInBourdaries(music.Valence, item.Valence) &&
                        AcousticnessInBourdaries(music.Acousticness, item.Acousticness)) //addig, amíg a hozzátartózó zenék vannak és számolunk egy távolságot, prioritásos sorba rakjuk ez alapján
                    {
                        double dist = EuclideanDistance(music, item);
                        closestMusics.Add(dist, item);
                        
                    }
                }
            }
            LinkedListNode current = closestMusics.Head;
            while (current != null)
            {
                if (LLMinsSum(selectedMusics) < minsLeft && (LLMinsSum(selectedMusics) + MsToMins(current.Object.DurationMs)) <= minsLeft)
                {
                    selectedMusics.Add(current.Object);
                }
                current = current.Next;
            }
            return selectedMusics;
        }

        private int LLMinsSum(List<Music> ml)
        {
            int mins = 0;
            foreach (var item in ml)
            {
                mins += MsToMins(item.DurationMs);
            }
            return mins;
        }

        private int MinsSum(LinkedList c)
        {
            int mins = 0;
            foreach (var item in c)
            {
                mins += MsToMins(c.Head.Object.DurationMs);
            }
            return mins;
        }
        private int MsToMins(int ms)
        {
            double conv = 1.6667E-5;
            return (int)conv * ms;
        }

        private double EuclideanDistance(Music currMusic, Music dbMusic)
        {
            double dist = Math.Sqrt(Math.Pow(currMusic.Energy - dbMusic.Energy, 2) +
                                Math.Pow(currMusic.Valence - dbMusic.Valence, 2) +
                                Math.Pow(currMusic.Acousticness - dbMusic.Acousticness, 2));
            return dist;
        }

        private double FMEuclideanDistance(Music currMusic, Music dbMusic)
        {
            double dist = Math.Sqrt(Math.Pow(currMusic.Energy - dbMusic.Energy, 2) +
                                Math.Pow(currMusic.Valence - dbMusic.Valence, 2) +
                                Math.Pow(currMusic.Acousticness - dbMusic.Acousticness, 2) +
                                 Math.Pow(currMusic.Danceability - dbMusic.Danceability, 2) +
                                  Math.Pow(currMusic.Tempo - dbMusic.Tempo, 2) +
                                   Math.Pow(currMusic.Speechiness - dbMusic.Speechiness, 2)
                                );
            return dist;
        }

        public async Task<IEnumerable<Music>> FindMore(string trackId)
        { 
            var choosenMusic = await _context.Musics.FirstOrDefaultAsync(x => x.TrackId == trackId);
            var songsToChooseFrom = new List<Music>();
            
            foreach (var music in _context.Musics.Where(x => x.Genre == choosenMusic.Genre && x.Mode == choosenMusic.Mode))
            {
                if (EverythingInBoundaries(music, choosenMusic))
                {
                    songsToChooseFrom.Add(music);
                }
            }

            LinkedList closestMusics = new LinkedList();

            foreach (var song in songsToChooseFrom)
            {
                double dist = EuclideanDistance(choosenMusic, song);
                closestMusics.Add(dist, song);
            }
            
            List<Music> selectedMusics = new List<Music>();

            LinkedListNode current = closestMusics.Head;
            int helper = 0;
            while (current != null && helper < 20)
            {
                selectedMusics.Add(current.Object);
                current = current.Next;
                helper++;
            }
            
            return selectedMusics;
        }

        private bool EverythingInBoundaries(Music music, Music? choosenMusic)
        {
            if (FMEnergyInBourdaries(music.Energy, choosenMusic.Energy) &&
                FMValenceInBourdaries(music.Valence, choosenMusic.Valence) &&
                FMAcousticnessInBourdaries(music.Acousticness, choosenMusic.Acousticness) &&
                FMDanceabilityInBourdaries(music.Danceability, choosenMusic.Danceability) &&
                FMSpeechinessInBoundaries(music.Speechiness, choosenMusic.Speechiness) &&
                FMTempoInBoundaries(music.Tempo, choosenMusic.Tempo))
            {
                return true;
            }
            else
                return false;
        }
        public async Task<Music> FindMusic(string trackId)
        {
            var music = await _context.Musics.FirstOrDefaultAsync(x => x.TrackId == trackId);

            return music;
        }

        public async Task<IEnumerable<Music>> Search(string expr)
        {
            string[] words = expr.Split(' ');
            Dictionary<Music, int> matchedWords = new Dictionary<Music, int>();
            int dictLength = 0;
           
            foreach (var music in _context.Musics)
            {
                int counter = 0;
                for (int i = 0; i < words.Length; i++)
                {
                    if (music.ArtistName.ToLower().Contains(words[i].ToLower()))
                    {
                        counter++;
                    }
                    if (music.TrackName.ToLower().Contains(words[i].ToLower()))
                    {
                        counter++;
                    }
                }

                if (counter > 0)
                {
                    matchedWords.Add(music, counter);
                    dictLength++;
                }
            }
            
            

            var sortedDict = from entry in matchedWords orderby entry.Value descending select entry;
            List<Music> musicsToReturn = new List<Music>();
            int max = RandomGenerator.rnd.Next(14, 21);

            foreach (var item in sortedDict)
            {
                if (musicsToReturn.Count() < max)
                {
                    musicsToReturn.Add(item.Key);
                }
                
            }

            return musicsToReturn;
            
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

        private bool FMEnergyInBourdaries(double currEnergy, double dbEnergy)
        {
            if (dbEnergy <= currEnergy + 0.15 && dbEnergy >= currEnergy - 0.15)
                return true;

            return false;
        }

        private bool FMValenceInBourdaries(double currValence, double dbValence)
        {
            if (dbValence <= currValence + 0.15 && dbValence >= currValence - 0.15)
                return true;

            return false;
        }

        private bool FMAcousticnessInBourdaries(double currAcousticness, double dbAcousticness)
        {
            if (dbAcousticness <= currAcousticness + 0.15 && dbAcousticness >= currAcousticness - 0.15)
                return true;

            return false;
        }

        private bool FMTempoInBoundaries(double currTempo, double dbTempo)
        {
            if (dbTempo <= currTempo + 8 && dbTempo >= currTempo - 8)
                return true;

            return false;
        }

        private bool FMSpeechinessInBoundaries(double currSpeechiness, double dbSpeechiness)
        {
            if (dbSpeechiness <= currSpeechiness + 0.15 && dbSpeechiness >= currSpeechiness - 0.15)
                return true;

            return false;
        }

        private bool FMDanceabilityInBourdaries(double currDanceability, double dbDanceability)
        {
            if (dbDanceability <= currDanceability + 0.15 && dbDanceability >= currDanceability - 0.15)
                return true;

            return false;
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

        public async Task<IEnumerable<Music>> GetLikedSongsInToD(int id)
        {
            var givenuser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            var musics = new List<Music>();
            var behaviours = new List<UserBehavior>();
            string nameOfDay = DateTime.Now.DayOfWeek.ToString();
            string timeOfDay = TimeOfDayConverter();

            foreach (var behav in _context.UserBehaviors)
            {
                if (id == behav.UserId && behav.TimeOfDay == timeOfDay && behav.NameOfDay == nameOfDay)
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
