﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System.Net.Http.Headers;
using UNN1N9_SOF_2022231_BACKEND.Data;
using UNN1N9_SOF_2022231_BACKEND.DTOs;
using UNN1N9_SOF_2022231_BACKEND.Errors;
using UNN1N9_SOF_2022231_BACKEND.Helpers;
using UNN1N9_SOF_2022231_BACKEND.Models;
using UNN1N9_SOF_2022231_BACKEND.Services;

namespace UNN1N9_SOF_2022231_BACKEND.Logic
{
    public class MusicLogic : IMusicLogic
    {
        IDataContext _context;
        private readonly SpotifySettings _spotifySettings;

        public MusicLogic(IDataContext context, IOptions<SpotifySettings> spotifySettings)
        {
            _context = context;
            this._spotifySettings = spotifySettings.Value;
        }

        #region lejatszasilistak

        public async Task<IEnumerable<Music>> GetPersonalizedMixCluster(int id)
        {
            var givenuser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            string nameOfDay = DateTime.Now.DayOfWeek.ToString();
            string timeOfDay = TimeOfDayConverter();
            var musics = new List<Music>();
            var behaviours = new List<UserBehavior>();
            var genres = new List<string>();
            var style = "";
            var styles = new List<string>();
            DateOnly Date = DateOnly.FromDateTime(DateTime.Now);

            foreach (var behav in _context.UserBehaviors)
            {
                var f = behav.UserId == givenuser.Id;
                var u = behav.NameOfDay.Equals(nameOfDay);
                var c = behav.TimeOfDay.Equals(timeOfDay);
                var k = behav.Date.Equals(Date.AddDays(-7));

                var y = behav.Date;
                var ou = Date.AddDays(-7);

                if (behav.UserId == givenuser.Id &&
                    behav.NameOfDay.Equals(nameOfDay)
                    && behav.TimeOfDay.Equals(timeOfDay)
                    && behav.Date.Equals(Date.AddDays(-7)))
                {
                    behaviours.Add(behav);
                }
            }

            if (behaviours.Count() == 0) //ha nincs a múlthéten 
            {
                //akkor először ránézünk az azelőtti hetire, mert a megszokott hallgatásból kimaradthat 1 hét valami miatt
                foreach (var behav in _context.UserBehaviors)
                {
                    if (behav.UserId == givenuser.Id &&
                        behav.NameOfDay.Equals(nameOfDay)
                        && behav.TimeOfDay.Equals(timeOfDay)
                        && behav.Date == Date.AddDays(-14))
                    {
                        behaviours.Add(behav);
                    }
                }

                //ha ott sincs, akkor visszább felesleges menni, az már nem szokás amit 2 hete nem csináltunk,
                // úgyhogy lekérjük Spotify-ról a nemrégiben játszott zenéket, és az alapján ajánlunk
                if (behaviours.Count() == 0)
                {
                    behaviours = await GetRecentlyPlayedTracks(id);
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

            int minsLeft = GetTimeLeftFromTimeOfDay(timeOfDay);

            if (behaviours.Count() == 0)
            {
                return GetRandomPlaylist(minsLeft);
            }
            else
            {
                // stílusok számának lekérdezése; ha 3 vagy több, akkor
                // vegyes mixet kap, ha kevesebb, akkor azt amiből a legtöbb van
                var dict = new Dictionary<string, int>();

                foreach (var music in musics)
                {
                    if (!genres.Contains(music.Genre))
                    {
                        genres.Add(music.Genre);
                        dict[music.Genre] = 1;
                        styles.Add(music.Genre);
                    }
                    else
                        dict[music.Genre] += 1;
                }

                // ha nem kevert, akkor a leghallgatottabb stílus kiszedése
                if (genres.Count <= 3)
                    style = dict.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;


                //itt ki van számítva, minsLeft hosszúságú lista kell vissza


                // leválogatjuk klaszterekbe a választható zenéket
                // de csak ha több a stílus, mint 3, különben csak 1 listába rakjuk bele
                var songCluster = new List<LinkedList>();
                var songsToChooseFrom = new LinkedList();
                var selectedStyles = new List<string>();

                foreach (var listenedMusic in musics)
                {
                    if (genres.Count < 3)
                    {
                        foreach (var otherMusic in _context.Musics
                            .Where(x => x.Genre == style && x.Mode == listenedMusic.Mode))
                        {
                            if (isWithinRange(listenedMusic, otherMusic)
                                && !LinkedListContains(songsToChooseFrom, otherMusic))
                                songsToChooseFrom.Add(
                                    EuclideanDistance(listenedMusic, otherMusic),
                                    otherMusic);
                        }
                    }
                    else // vegyes stílus
                    {
                        foreach (var otherMusic in _context.Musics
                            .Where(x => x.Genre == listenedMusic.Genre && x.Mode == listenedMusic.Mode))
                        {
                            // euklidészi távolság alapján mindegyik klaszter egy prioritásos sor lesz
                            if (isWithinRange(listenedMusic, otherMusic)
                                && !LinkedListClusterContains(songCluster, otherMusic))
                            {
                                // ha még nem szerepel a stílus a klaszterben, akkor létrehozza a klasztert
                                // különben pedig csak hozzáadja a megfelelő klaszterbe
                                if (!isGenreAlreadyACluster(songCluster, otherMusic.Genre))
                                    AddNewCluster(songCluster, listenedMusic, otherMusic);
                                else
                                    AddToCurrentCluster(songCluster, listenedMusic, otherMusic);
                            }
                        }
                    }
                }


                var selectedMusics = new List<Music>();
                LinkedListNode current = new LinkedListNode();
                LinkedList songsToChooseFromMixed = new LinkedList();
                double chooseableLength = 0;

                if (genres.Count < 3)
                {
                    chooseableLength = GetAllLengthOneGenre(songsToChooseFrom);
                    current = songsToChooseFrom.Head;
                }
                else
                {
                    chooseableLength = GetAllLengthMixedGenre(songCluster);
                    // akkor is belead minden zenéből, ha nem jön ki időben
                    selectedMusics = GetSongsFromEveryGenre(songCluster);

                    // ha még van hátralevő idő, akkor "kilapítjuk" és 1 nagy prioritásos sort csinálunk a klaszterekből
                    if (LLMinsSum(selectedMusics) < minsLeft)
                    {
                        songsToChooseFromMixed = MakePriorityQueueFromAllSongs(songCluster);
                        current = songsToChooseFromMixed.Head;
                    }

                }


                // utána pedig mohó módon feltöljük a viszaadott listát

                int minCounter = 0;

                while (current != null)
                {
                    if (LLMinsSum(selectedMusics) < minsLeft && (LLMinsSum(selectedMusics) + MsToMins(current.Object.DurationMs)) <= minsLeft && !selectedMusics.Contains(current.Object))
                    {
                        selectedMusics.Add(current.Object);
                        minCounter += MsToMins(current.Object.DurationMs);
                    }
                    current = current.Next;
                }

                return selectedMusics;
            }
        }

        public async Task<IEnumerable<Music>> FindMusic(string trackId)
        {
            var music = await _context.Musics.FirstOrDefaultAsync(x => x.TrackId == trackId);

            var musics = new List<Music>();
            musics.Add(music);

            return musics;
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

        public async Task<IEnumerable<Music>> GetActualLikedSongs(int id)
        {
            var givenuser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            var musics = new List<Music>();
            var behaviours = new List<LikedSong>();
            foreach (var behav in _context.LikedSongs)
            {
                if (id == behav.UserId)
                {
                    behaviours.Add(behav);
                }
            }
            foreach (var music in _context.Musics)
            {
                if (ContainsLikedSong(music.Id, behaviours))
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
                    if (musics.Count() < 50)
                    {
                        musics.Add(music);
                    }
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
                    if (musics.Count() < 50)
                    {
                        musics.Add(music);
                    }
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
                    if (musics.Count() < 50)
                    {
                        musics.Add(music);
                    }
                }
            }

            return musics;
        }

        public async Task<IEnumerable<Music>> FindMoreByArtist(string expr)
        {
            var musicsToReturn = new List<Music>();
            foreach (var item in _context.Musics)
            {
                if (item.ArtistName == expr || item.TrackName.Contains(expr))
                {
                    musicsToReturn.Add(item);
                }
            }

            return musicsToReturn;
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
            var styles = new List<string>();
            DateOnly Date = DateOnly.FromDateTime(DateTime.Now);

            foreach (var behav in _context.UserBehaviors)
            {
                if (behav.UserId == givenuser.Id &&
                    behav.NameOfDay.Equals(nameOfDay)
                    && behav.TimeOfDay.Equals(timeOfDay)
                    && behav.Date == Date.AddDays(-7))
                {
                    behaviours.Add(behav);
                }
            }

            if (behaviours.Count() == 0) //ha nincs a múlthéten 
            {
                //akkor először ránézünk az azelőtti hetire, mert a megszokott hallgatásból kimaradthat 1 hét valami miatt
                foreach (var behav in _context.UserBehaviors)
                {
                    if (behav.UserId == givenuser.Id &&
                        behav.NameOfDay.Equals(nameOfDay)
                        && behav.TimeOfDay.Equals(timeOfDay)
                        && behav.Date == Date.AddDays(-14))
                    {
                        behaviours.Add(behav);
                    }
                }

                //ha ott sincs, akkor visszább felesleges menni, az már nem szokás amit 2 hete nem csináltunk,
                // úgyhogy lekérjük Spotify-ról a nemrégiben játszott zenéket, és az alapján ajánlunk
                if (behaviours.Count() == 0)
                {
                    behaviours = await GetRecentlyPlayedTracks(id);
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

            // ha üres, akkor lekérjük Spotify API segítségével a 'Get Recently Played Tracks' endponttal a nemrégiben
            // lejátszott zenéit, hozzáadjuk a Behaviour-eihez (ha nincs benne a zenékhez is), és ez alapján csináljuk

            // ha még ezután is üres, akkor véletlenszerű listát adunk vissza

            //a napszakból való hátramaradó idő kiszámítása
            int endHour = EndHour(timeOfDay);
            int currHour = DateTime.Now.Hour;
            int currMins = DateTime.Now.Minute;

            int hoursLeft = 0;
            int minsLeft = 0;

            if (currMins != 0)
                hoursLeft = endHour - currHour - 1;
            else
                hoursLeft = endHour - currHour;

            minsLeft = 60 - currMins;

            if (hoursLeft != 0)
                minsLeft = minsLeft + hoursLeft * 60;


            if (behaviours.Count() == 0)
            {
                return GetRandomPlaylist(minsLeft);
            }
            else
            {
                // stílusok számának lekérdezése; ha 3 vagy több, akkor
                // vegyes mixet kap, ha kevesebb, akkor azt amiből a legtöbb van
                var dict = new Dictionary<string, int>();

                foreach (var music in musics)
                {
                    if (!genres.Contains(music.Genre))
                    {
                        genres.Add(music.Genre);
                        dict[music.Genre] = 1;
                        styles.Add(music.Genre);
                    }
                    else
                        dict[music.Genre] += 1;
                }

                // ha nem kevert, akkor a leghallgatottabb stílus kiszedése
                if (genres.Count <= 3)
                    style = dict.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;


                //itt ki van számítva, minsLeft hosszúságú lista kell vissza

                var selectedMusics = new List<Music>();
                var songsToChooseFrom = new List<Music>();

                if (genres.Count >= 3) //kevert megoldás
                {
                    foreach (var music in musics)
                    {
                        foreach (var allM in _context.Musics.Where(x => x.Genre == music.Genre && x.Mode == music.Mode))
                        {
                            if (EnergyInBourdaries(music.Energy, allM.Energy)
                                && ValenceInBourdaries(music.Valence, allM.Valence) &&
                                AcousticnessInBourdaries(music.Acousticness, allM.Acousticness))
                            {
                                songsToChooseFrom.Add(allM);
                            }
                        }
                    }
                }
                else //nem kevert megoldás
                {
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
                }

                //le van szűrve az összes választható szám - márcsak végig kell menni,
                //euklidészi távolságot számolni, prioritásos sorba rakni és a kimeneti zenékbe rakni amíg belefér -> lehetne láncolt lista is, ahol távolság alapján szúrjuk be

                LinkedList closestMusics = new LinkedList();
                var checker = new List<Music>();

                foreach (var music in musics.Distinct()) //végigmegyünk a zenéken
                {
                    foreach (var item in songsToChooseFrom) //és a választható zenéken
                    {
                        if (!checker.Contains(item))
                        {
                            double dist = EuclideanDistance(music, item);
                            closestMusics.Add(dist, item);
                            checker.Add(item);
                        }
                    }
                }

                LinkedListNode current = closestMusics.Head;

                int chooseableLength = 0;
                foreach (var item in songsToChooseFrom)
                {
                    chooseableLength += MsToMins(item.DurationMs);
                }

                int minCounter = 0;
                while (current != null)
                {
                    if (LLMinsSum(selectedMusics) < minsLeft && (LLMinsSum(selectedMusics) + MsToMins(current.Object.DurationMs)) <= minsLeft)
                    {
                        selectedMusics.Add(current.Object);
                        minCounter += MsToMins(current.Object.DurationMs);
                    }
                    current = current.Next;
                }

                return selectedMusics;
            }


        }

        private List<Music> GetRandomPlaylist(int minsLeft)
        {
            var songs = new List<Music>();

            int totalMusicCount = _context.Musics.Count();

            while (LLMinsSum(songs) < minsLeft)
            {
                int randomIndex = RandomGenerator.rnd.Next(totalMusicCount);
                var song = _context.Musics
                    .OrderBy(m => m.Id)
                    .Skip(randomIndex)
                    .FirstOrDefault();

                if (LLMinsSum(songs) + MsToMins(song.DurationMs) <= minsLeft)
                {
                    songs.Add(song);
                }
            }

            return songs;
        }

        private async Task<List<UserBehavior>> GetRecentlyPlayedTracks(int id)
        {
            var givenuser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            var behavsToReturn = new List<UserBehavior>();

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", givenuser?.SpotifyAccessToken);
            var response = await httpClient.GetAsync("https://api.spotify.com/v1/me/player/recently-played");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var recentlyPlayed = JsonConvert.DeserializeObject<RecentlyPlayedDto>(responseContent);
                var trackIds = recentlyPlayed?.Items?.Select(item => item.Track?.Id).ToList();

                //megnézni, hogy melyek azok, amelyek nincsenek benne az adatbázisban
                if (trackIds.Count() != 0) //ha kaptunk vissza zenéket
                {
                    foreach (var item in trackIds) //akkor ezeken végigmegyünk
                    {
                        if (!ContainsMusicInDB(item)) //ha nincs benne az adatbázisban a zene, akkor hozzáadjuk
                        {
                            var x = await AddSong(givenuser.Id, item);
                        }
                        //majd a Behavior táblához is
                        AccessTokenDTO dto = new AccessTokenDTO() { userid = givenuser.Id, token = item };
                        var b = await AddBehaviorWithButton(dto);
                        behavsToReturn.Add(b);

                    }
                }
            }
            ;
            return behavsToReturn;
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

            //Get5ClosestSongs(closestMusics); //csak a tesztelés miatt van

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

        private void Get5ClosestSongs(LinkedList closestMusics)
        {
            LinkedListNode current = closestMusics.Head;
            double[] distances = new double[6];
            int counter = 0;
            while (current != null)
            {
                Console.WriteLine(current.Object.ArtistName + ": " + current.Object.TrackName + " >>> " + Math.Round(current.Value, 4));
                if (counter == 5)
                {
                    distances[counter] = Math.Round(current.Value, 4);
                    current = null;
                }
                else
                {
                    distances[counter] = Math.Round(current.Value, 4);
                    current = current.Next;
                    counter++;
                }
            }
            double sum = 0;
            for (int i = 1; i < distances.Length; i++)
            {
                sum += distances[i];
            }

            double x = (double)sum / 5;
            Console.WriteLine("Mean: " + Math.Round(x, 4));

        }

        #endregion

        #region statisztika

        public async Task<IEnumerable<StatDto>> GetDailyStatistics(int id)
        {
            var givenuser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            string nameOfDay = DateTime.Now.DayOfWeek.ToString();
            var statstoReturn = new List<StatDto>();
            var behaviours = new List<UserBehavior>();
            var stat = new StatDto();
            var dict = new Dictionary<string, int>();
            var genres = new List<string>();

            // összegyűjtjük az aznapra vonatkozó adatokat
            foreach (var behavior in _context.UserBehaviors.Where(x => x.UserId == givenuser.Id && x.NameOfDay == nameOfDay))
            {
                behaviours.Add(behavior);
            }

            // ha nincs aznapra való adat, akkor alapértelmezett beállítása
            if (behaviours.Count() == 0)
            {
                stat.MostListenedGenre = "-";
                stat.MinsSpent = 0;
                stat.MostListenedArtist = "-";
                stat.MostListenedSong = "-";
                stat.NumOfListenedGenre = 0;
                stat.Type = "Daily";
            }
            else
            {
                // van adat


                // leghallgatottabb stílusok kiszámolása                   
                foreach (var behavior in behaviours)
                {
                    var music = _context.Musics.FirstOrDefault(x => x.Id == behavior.MusicId);
                    if (!genres.Contains(music.Genre))
                    {
                        genres.Add(music.Genre);
                        dict[music.Genre] = behavior.ListeningCount;
                    }
                    else
                    {
                        dict[music.Genre] += 1 * behavior.ListeningCount;
                    }
                }


                //minsSpent
                var minsSpent = 0;
                foreach (var item in behaviours)
                {
                    var music = _context.Musics.FirstOrDefault(x => x.Id == item.MusicId);
                    minsSpent += (int)Math.Round((StatMsToMins(music.DurationMs) * item.ListeningCount));
                }

                stat.MinsSpent = minsSpent;

                //mostListenedArtist
                var mostListenedArtist =
                    from x in behaviours
                    group x by GetMusic(x).ArtistName into g
                    select new KeyValuePair<string, double>
                    (g.Key, g.Sum(y => y.ListeningCount * StatMsToMins(GetMusic(y).DurationMs)));

                //stat.MostListenedArtist = mostListenedArtist.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

                //mostListenedSong
                var mostListenedSong = from x in behaviours
                                       group x by GetMusic(x).TrackName into g
                                       select new KeyValuePair<string, double>
                                       (g.Key, g.Sum(y => y.ListeningCount));


                //numOfListednedGenre

                stat.NumOfListenedGenre = dict.Count();
                stat.Type = "Daily";

                // 5 legjobb mindenből
                var topGenres = dict.OrderByDescending(kv => kv.Value).Take(5).Select(kv => kv.Key).ToList();
                var topArtists = mostListenedArtist.OrderByDescending(kv => kv.Value).Take(5).Select(kv => kv.Key).ToList();
                var topSongs = mostListenedSong.OrderByDescending(kv => kv.Value).Take(5).Select(kv => kv.Key).ToList();

                // ha valami null, akkor kicseréljük a '-' karakterre
                stat.MostListenedGenre = topGenres.ElementAtOrDefault(0) ?? "-";
                stat.SecondMostListenedGenre = topGenres.ElementAtOrDefault(1) ?? "-";
                stat.ThirdMostListenedGenre = topGenres.ElementAtOrDefault(2) ?? "-";
                stat.FourthMostListenedGenre = topGenres.ElementAtOrDefault(3) ?? "-";
                stat.FifthMostListenedGenre = topGenres.ElementAtOrDefault(4) ?? "-";

                // további kitöltések a leghallgatottabb előadókhoz és zenékhez

                stat.MostListenedArtist = topArtists.ElementAtOrDefault(0) ?? "-";
                stat.SecondMostListenedArtist = topArtists.ElementAtOrDefault(1) ?? "-";
                stat.ThirdMostListenedArtist = topArtists.ElementAtOrDefault(2) ?? "-";
                stat.FourthMostListenedArtist = topArtists.ElementAtOrDefault(3) ?? "-";
                stat.FifthMostListenedArtist = topArtists.ElementAtOrDefault(4) ?? "-";

                stat.MostListenedSong = topSongs.ElementAtOrDefault(0) ?? "-";
                stat.SecondMostListenedSong = topSongs.ElementAtOrDefault(1) ?? "-";
                stat.ThirdMostListenedSong = topSongs.ElementAtOrDefault(2) ?? "-";
                stat.FourthMostListenedSong = topSongs.ElementAtOrDefault(3) ?? "-";
                stat.FifthMostListenedSong = topSongs.ElementAtOrDefault(4) ?? "-";


            }

            statstoReturn.Add(stat);

            return statstoReturn;
        }

        /*
        private async Task<IEnumerable<StatDto>> GetDailyStatisticsOLD(int id)
        {
            var givenuser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            string nameOfDay = DateTime.Now.DayOfWeek.ToString();
            var statstoReturn = new List<StatDto>();
            var behaviours = new List<UserBehavior>();
            var stat = new StatDto();
            var dict = new Dictionary<string, int>();
            var genres = new List<string>();

            //kiszedjük a napra vonatkozó adatokat
            foreach (var item in _context.UserBehaviors.Where(x => x.UserId == givenuser.Id && x.NameOfDay == nameOfDay))
            {
                behaviours.Add(item);
            }

            if (behaviours.Count() == 0)
            {
                stat.MostListenedGenre = "-";
                stat.MinsSpent = 0;
                stat.MostListenedArtist = "-";
                stat.MostListenedSong = "-";
                stat.NumOfListenedGenre = 0;
                stat.Type = "Daily";
            }
            else
            {
                //mostListenedGenre           

                foreach (var item in behaviours)
                {
                    var music = _context.Musics.FirstOrDefault(x => x.Id == item.MusicId);
                    if (!genres.Contains(music.Genre))
                    {
                        genres.Add(music.Genre);
                        dict[music.Genre] = item.ListeningCount;
                    }
                    else
                    {
                        dict[music.Genre] += 1 * item.ListeningCount;
                    }
                }
                stat.MostListenedGenre = dict.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

                //minsSpent
                var minsSpent = 0;
                foreach (var item in behaviours)
                {
                    var music = _context.Musics.FirstOrDefault(x => x.Id == item.MusicId);
                    minsSpent += (int)Math.Round((StatMsToMins(music.DurationMs) * item.ListeningCount));
                }

                stat.MinsSpent = minsSpent;

                //mostListenedArtist
                var mostListenedArtist = from x in behaviours
                                         group x by x.Music.ArtistName into g
                                         select new KeyValuePair<string, double>
                                         (g.Key, g.Sum(y => y.ListeningCount * StatMsToMins(y.Music.DurationMs)));

                stat.MostListenedArtist = mostListenedArtist.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

                //mostListenedSong

                var mostListenedSong = from x in behaviours
                                       group x by x.Music.TrackName into g
                                       select new KeyValuePair<string, double>
                                       (g.Key, g.Sum(y => y.ListeningCount));

                stat.MostListenedSong = mostListenedSong.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

                //numOfListednedGenre

                stat.NumOfListenedGenre = dict.Count();
                stat.Type = "Daily";
            }

            statstoReturn.Add(stat);

            return statstoReturn;
        } 
        
         */

        public async Task<IEnumerable<StatDto>> GetWeeklyStatistics(int id)
        {
            var givenuser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            var statstoReturn = new List<StatDto>();
            var behaviours = new List<UserBehavior>();
            var stat = new StatDto();
            var dict = new Dictionary<string, int>();
            var genres = new List<string>();
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            //kiszedjük az elmúlt egy hét (a mai naptól visszamenőleg 1 hétre)
            foreach (var item in _context.UserBehaviors.Where(x => x.Date > today.AddDays(-7) && x.UserId == givenuser.Id))
            {
                behaviours.Add(item);
            }

            if (behaviours.Count() == 0)
            {
                stat.MostListenedGenre = "-";
                stat.MinsSpent = 0;
                stat.MostListenedArtist = "-";
                stat.MostListenedSong = "-";
                stat.NumOfListenedGenre = 0;
                stat.Type = "Weekly";
            }
            else
            {
                //mostListenedGenre
                foreach (var item in behaviours)
                {
                    var music = _context.Musics.FirstOrDefault(x => x.Id == item.MusicId);
                    if (!genres.Contains(music.Genre))
                    {
                        genres.Add(music.Genre);
                        dict[music.Genre] = item.ListeningCount;
                    }
                    else
                    {
                        dict[music.Genre] += 1 * item.ListeningCount;
                    }
                }
                stat.MostListenedGenre = dict.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

                //minsSpent
                var minsSpent = 0;
                foreach (var item in behaviours)
                {
                    var music = _context.Musics.FirstOrDefault(x => x.Id == item.MusicId);
                    minsSpent += (int)Math.Round((StatMsToMins(music.DurationMs) * item.ListeningCount));
                }

                stat.MinsSpent = minsSpent;

                //mostListenedArtist
                var mostListenedArtist = from x in behaviours
                                         group x by GetMusic(x).ArtistName into g
                                         select new KeyValuePair<string, double>
                                         (g.Key, g.Sum(y => y.ListeningCount * StatMsToMins(GetMusic(y).DurationMs)));

                stat.MostListenedArtist = mostListenedArtist.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

                //mostListenedSong
                var mostListenedSong = from x in behaviours
                                       group x by GetMusic(x).TrackName into g
                                       select new KeyValuePair<string, double>
                                       (g.Key, g.Sum(y => y.ListeningCount));

                stat.MostListenedSong = mostListenedSong.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

                //numOfListednedGenre

                stat.NumOfListenedGenre = dict.Count();
                stat.Type = "Weekly";

                var topGenres = dict.OrderByDescending(kv => kv.Value).Take(5).Select(kv => kv.Key).ToList();
                var topArtists = mostListenedArtist.OrderByDescending(kv => kv.Value).Take(5).Select(kv => kv.Key).ToList();
                var topSongs = mostListenedSong.OrderByDescending(kv => kv.Value).Take(5).Select(kv => kv.Key).ToList();

                stat.MostListenedGenre = topGenres.ElementAtOrDefault(0) ?? "-";
                stat.SecondMostListenedGenre = topGenres.ElementAtOrDefault(1) ?? "-";
                stat.ThirdMostListenedGenre = topGenres.ElementAtOrDefault(2) ?? "-";
                stat.FourthMostListenedGenre = topGenres.ElementAtOrDefault(3) ?? "-";
                stat.FifthMostListenedGenre = topGenres.ElementAtOrDefault(4) ?? "-";

                stat.MostListenedArtist = topArtists.ElementAtOrDefault(0) ?? "-";
                stat.SecondMostListenedArtist = topArtists.ElementAtOrDefault(1) ?? "-";
                stat.ThirdMostListenedArtist = topArtists.ElementAtOrDefault(2) ?? "-";
                stat.FourthMostListenedArtist = topArtists.ElementAtOrDefault(3) ?? "-";
                stat.FifthMostListenedArtist = topArtists.ElementAtOrDefault(4) ?? "-";

                stat.MostListenedSong = topSongs.ElementAtOrDefault(0) ?? "-";
                stat.SecondMostListenedSong = topSongs.ElementAtOrDefault(1) ?? "-";
                stat.ThirdMostListenedSong = topSongs.ElementAtOrDefault(2) ?? "-";
                stat.FourthMostListenedSong = topSongs.ElementAtOrDefault(3) ?? "-";
                stat.FifthMostListenedSong = topSongs.ElementAtOrDefault(4) ?? "-";
            }

            statstoReturn.Add(stat);

            return statstoReturn;
        }

        /*
         
        private async Task<IEnumerable<StatDto>> GetWeeklyStatisticsOLD(int id)
        {
            var givenuser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            var statstoReturn = new List<StatDto>();
            var behaviours = new List<UserBehavior>();
            var stat = new StatDto();
            var dict = new Dictionary<string, int>();
            var genres = new List<string>();
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            //kiszedjük az elmúlt egy hét (a mai naptól visszamenőleg 1 hétre)
            foreach (var item in _context.UserBehaviors.Where(x => x.Date >= today.AddDays(-7) && x.UserId == givenuser.Id))
            {
                behaviours.Add(item);
            }

            if (behaviours.Count() == 0)
            {
                stat.MostListenedGenre = "-";
                stat.MinsSpent = 0;
                stat.MostListenedArtist = "-";
                stat.MostListenedSong = "-";
                stat.NumOfListenedGenre = 0;
                stat.Type = "Weekly";
            }
            else
            {
                //mostListenedGenre
                foreach (var item in behaviours)
                {
                    var music = _context.Musics.FirstOrDefault(x => x.Id == item.MusicId);
                    if (!genres.Contains(music.Genre))
                    {
                        genres.Add(music.Genre);
                        dict[music.Genre] = item.ListeningCount;
                    }
                    else
                    {
                        dict[music.Genre] += 1 * item.ListeningCount;
                    }
                }
                stat.MostListenedGenre = dict.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

                //minsSpent
                var minsSpent = 0;
                foreach (var item in behaviours)
                {
                    var music = _context.Musics.FirstOrDefault(x => x.Id == item.MusicId);
                    minsSpent += (int)Math.Round((StatMsToMins(music.DurationMs) * item.ListeningCount));
                }

                stat.MinsSpent = minsSpent;

                //mostListenedArtist
                var mostListenedArtist = from x in behaviours
                                         group x by x.Music.ArtistName into g
                                         select new KeyValuePair<string, double>
                                         (g.Key, g.Sum(y => y.ListeningCount * StatMsToMins(y.Music.DurationMs)));

                stat.MostListenedArtist = mostListenedArtist.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

                //mostListenedSong

                var mostListenedSong = from x in behaviours
                                       group x by x.Music.TrackName into g
                                       select new KeyValuePair<string, double>
                                       (g.Key, g.Sum(y => y.ListeningCount));

                stat.MostListenedSong = mostListenedSong.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

                //numOfListednedGenre

                stat.NumOfListenedGenre = dict.Count();
                stat.Type = "Weekly";
            }

            statstoReturn.Add(stat);

            return statstoReturn;
        }

        */

        public async Task<IEnumerable<StatDto>> GetMonthlyStatistics(int id)
        {
            var givenuser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            var statstoReturn = new List<StatDto>();
            var behaviours = new List<UserBehavior>();
            var stat = new StatDto();
            var dict = new Dictionary<string, int>();
            var genres = new List<string>();
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            //kiszedjük a jelenlegi hónap adatait
            foreach (var item in _context.UserBehaviors.Where(x => x.Date.Month == today.Month && x.UserId == givenuser.Id))
            {
                behaviours.Add(item);
            }

            if (behaviours.Count() == 0)
            {
                stat.MostListenedGenre = "-";
                stat.MinsSpent = 0;
                stat.MostListenedArtist = "-";
                stat.MostListenedSong = "-";
                stat.NumOfListenedGenre = 0;
                stat.Type = "Monthly";
            }
            else
            {

                //mostListenedGenre
                foreach (var item in behaviours)
                {
                    var music = _context.Musics.FirstOrDefault(x => x.Id == item.MusicId);
                    if (!genres.Contains(music.Genre))
                    {
                        genres.Add(music.Genre);
                        dict[music.Genre] = item.ListeningCount;
                    }
                    else
                    {
                        dict[music.Genre] += 1 * item.ListeningCount;
                    }
                }
                stat.MostListenedGenre = dict.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

                //minsSpent
                var minsSpent = 0;
                foreach (var item in behaviours)
                {
                    var music = _context.Musics.FirstOrDefault(x => x.Id == item.MusicId);
                    minsSpent += (int)Math.Round((StatMsToMins(music.DurationMs) * item.ListeningCount));
                }

                stat.MinsSpent = minsSpent;

                //mostListenedArtist
                var mostListenedArtist = from x in behaviours
                                         group x by GetMusic(x).ArtistName into g
                                         select new KeyValuePair<string, double>
                                         (g.Key, g.Sum(y => y.ListeningCount * StatMsToMins(GetMusic(y).DurationMs)));

                stat.MostListenedArtist = mostListenedArtist.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

                //mostListenedSong
                var mostListenedSong = from x in behaviours
                                       group x by GetMusic(x).TrackName into g
                                       select new KeyValuePair<string, double>
                                       (g.Key, g.Sum(y => y.ListeningCount));

                stat.MostListenedSong = mostListenedSong.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

                //numOfListednedGenre

                stat.NumOfListenedGenre = dict.Count();
                stat.Type = "Monthly";

                var topGenres = dict.OrderByDescending(kv => kv.Value).Take(5).Select(kv => kv.Key).ToList();
                var topArtists = mostListenedArtist.OrderByDescending(kv => kv.Value).Take(5).Select(kv => kv.Key).ToList();
                var topSongs = mostListenedSong.OrderByDescending(kv => kv.Value).Take(5).Select(kv => kv.Key).ToList();

                stat.MostListenedGenre = topGenres.ElementAtOrDefault(0) ?? "-";
                stat.SecondMostListenedGenre = topGenres.ElementAtOrDefault(1) ?? "-";
                stat.ThirdMostListenedGenre = topGenres.ElementAtOrDefault(2) ?? "-";
                stat.FourthMostListenedGenre = topGenres.ElementAtOrDefault(3) ?? "-";
                stat.FifthMostListenedGenre = topGenres.ElementAtOrDefault(4) ?? "-";

                stat.MostListenedArtist = topArtists.ElementAtOrDefault(0) ?? "-";
                stat.SecondMostListenedArtist = topArtists.ElementAtOrDefault(1) ?? "-";
                stat.ThirdMostListenedArtist = topArtists.ElementAtOrDefault(2) ?? "-";
                stat.FourthMostListenedArtist = topArtists.ElementAtOrDefault(3) ?? "-";
                stat.FifthMostListenedArtist = topArtists.ElementAtOrDefault(4) ?? "-";

                stat.MostListenedSong = topSongs.ElementAtOrDefault(0) ?? "-";
                stat.SecondMostListenedSong = topSongs.ElementAtOrDefault(1) ?? "-";
                stat.ThirdMostListenedSong = topSongs.ElementAtOrDefault(2) ?? "-";
                stat.FourthMostListenedSong = topSongs.ElementAtOrDefault(3) ?? "-";
                stat.FifthMostListenedSong = topSongs.ElementAtOrDefault(4) ?? "-";
            }

            statstoReturn.Add(stat);

            return statstoReturn;
        }

        /*
        private async Task<IEnumerable<StatDto>> GetMonthlyStatisticsOLD(int id)
        {
            var givenuser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            var statstoReturn = new List<StatDto>();
            var behaviours = new List<UserBehavior>();
            var stat = new StatDto();
            var dict = new Dictionary<string, int>();
            var genres = new List<string>();
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            //kiszedjük a jelenlegi hónap adatait
            foreach (var item in _context.UserBehaviors.Where(x => x.Date.Month == today.Month && x.UserId == givenuser.Id))
            {
                behaviours.Add(item);
            }

            if (behaviours.Count() == 0)
            {
                stat.MostListenedGenre = "-";
                stat.MinsSpent = 0;
                stat.MostListenedArtist = "-";
                stat.MostListenedSong = "-";
                stat.NumOfListenedGenre = 0;
                stat.Type = "Monthly";
            }
            else
            {

                //mostListenedGenre
                foreach (var item in behaviours)
                {
                    var music = _context.Musics.FirstOrDefault(x => x.Id == item.MusicId);
                    if (!genres.Contains(music.Genre))
                    {
                        genres.Add(music.Genre);
                        dict[music.Genre] = item.ListeningCount;
                    }
                    else
                    {
                        dict[music.Genre] += 1 * item.ListeningCount;
                    }
                }
                stat.MostListenedGenre = dict.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

                //minsSpent
                var minsSpent = 0;
                foreach (var item in behaviours)
                {
                    var music = _context.Musics.FirstOrDefault(x => x.Id == item.MusicId);
                    minsSpent += (int)Math.Round((StatMsToMins(music.DurationMs) * item.ListeningCount));
                }

                stat.MinsSpent = minsSpent;

                //mostListenedArtist
                var mostListenedArtist = from x in behaviours
                                         group x by x.Music.ArtistName into g
                                         select new KeyValuePair<string, double>
                                         (g.Key, g.Sum(y => y.ListeningCount * StatMsToMins(y.Music.DurationMs)));

                stat.MostListenedArtist = mostListenedArtist.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

                //mostListenedSong

                var mostListenedSong = from x in behaviours
                                       group x by x.Music.TrackName into g
                                       select new KeyValuePair<string, double>
                                       (g.Key, g.Sum(y => y.ListeningCount));

                stat.MostListenedSong = mostListenedSong.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

                //numOfListednedGenre

                stat.NumOfListenedGenre = dict.Count();
                stat.Type = "Monthly";
            }

            statstoReturn.Add(stat);

            return statstoReturn;
        }

        */

        public async Task<IEnumerable<StatDto>> GetYearlyStatistics(int id)
        {
            var givenuser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            var statstoReturn = new List<StatDto>();
            var behaviours = new List<UserBehavior>();
            var stat = new StatDto();
            var dict = new Dictionary<string, int>();
            var genres = new List<string>();
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            var xxx = today.Year;

            //kiszedjük a jelenlegi év adatait
            foreach (var item in _context.UserBehaviors.Where(x => x.Date.Year == today.Year && x.UserId == givenuser.Id))
            {
                behaviours.Add(item);
            }

            if (behaviours.Count() == 0)
            {
                stat.MostListenedGenre = "-";
                stat.MinsSpent = 0;
                stat.MostListenedArtist = "-";
                stat.MostListenedSong = "-";
                stat.NumOfListenedGenre = 0;
                stat.Type = "Yearly";
            }
            else
            {

                //mostListenedGenre
                foreach (var item in behaviours)
                {
                    var music = _context.Musics.FirstOrDefault(x => x.Id == item.MusicId);
                    if (!genres.Contains(music.Genre))
                    {
                        genres.Add(music.Genre);
                        dict[music.Genre] = item.ListeningCount;
                    }
                    else
                    {
                        dict[music.Genre] += 1 * item.ListeningCount;
                    }
                }
                stat.MostListenedGenre = dict.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

                //minsSpent
                var minsSpent = 0;
                foreach (var item in behaviours)
                {
                    var music = _context.Musics.FirstOrDefault(x => x.Id == item.MusicId);
                    minsSpent += (int)Math.Round((StatMsToMins(music.DurationMs) * item.ListeningCount));
                }

                stat.MinsSpent = minsSpent;

                //mostListenedArtist
                var mostListenedArtist = from x in behaviours
                                         group x by GetMusic(x).ArtistName into g
                                         select new KeyValuePair<string, double>
                                         (g.Key, g.Sum(y => y.ListeningCount * StatMsToMins(GetMusic(y).DurationMs)));

                stat.MostListenedArtist = mostListenedArtist.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

                //mostListenedSong
                var mostListenedSong = from x in behaviours
                                       group x by GetMusic(x).TrackName into g
                                       select new KeyValuePair<string, double>
                                       (g.Key, g.Sum(y => y.ListeningCount));

                stat.MostListenedSong = mostListenedSong.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

                //numOfListednedGenre

                stat.NumOfListenedGenre = dict.Count();
                stat.Type = "Yearly";

                var topGenres = dict.OrderByDescending(kv => kv.Value).Take(5).Select(kv => kv.Key).ToList();
                var topArtists = mostListenedArtist.OrderByDescending(kv => kv.Value).Take(5).Select(kv => kv.Key).ToList();
                var topSongs = mostListenedSong.OrderByDescending(kv => kv.Value).Take(5).Select(kv => kv.Key).ToList();

                stat.MostListenedGenre = topGenres.ElementAtOrDefault(0) ?? "-";
                stat.SecondMostListenedGenre = topGenres.ElementAtOrDefault(1) ?? "-";
                stat.ThirdMostListenedGenre = topGenres.ElementAtOrDefault(2) ?? "-";
                stat.FourthMostListenedGenre = topGenres.ElementAtOrDefault(3) ?? "-";
                stat.FifthMostListenedGenre = topGenres.ElementAtOrDefault(4) ?? "-";

                stat.MostListenedArtist = topArtists.ElementAtOrDefault(0) ?? "-";
                stat.SecondMostListenedArtist = topArtists.ElementAtOrDefault(1) ?? "-";
                stat.ThirdMostListenedArtist = topArtists.ElementAtOrDefault(2) ?? "-";
                stat.FourthMostListenedArtist = topArtists.ElementAtOrDefault(3) ?? "-";
                stat.FifthMostListenedArtist = topArtists.ElementAtOrDefault(4) ?? "-";

                stat.MostListenedSong = topSongs.ElementAtOrDefault(0) ?? "-";
                stat.SecondMostListenedSong = topSongs.ElementAtOrDefault(1) ?? "-";
                stat.ThirdMostListenedSong = topSongs.ElementAtOrDefault(2) ?? "-";
                stat.FourthMostListenedSong = topSongs.ElementAtOrDefault(3) ?? "-";
                stat.FifthMostListenedSong = topSongs.ElementAtOrDefault(4) ?? "-";
            }

            statstoReturn.Add(stat);

            return statstoReturn;
        }

        /*

        private async Task<IEnumerable<StatDto>> GetYearlyStatisticsOLD(int id)
        {
            var givenuser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            var statstoReturn = new List<StatDto>();
            var behaviours = new List<UserBehavior>();
            var stat = new StatDto();
            var dict = new Dictionary<string, int>();
            var genres = new List<string>();
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            var xxx = today.Year;

            //kiszedjük a jelenlegi év adatait
            foreach (var item in _context.UserBehaviors.Where(x => x.Date.Year == today.Year && x.UserId == givenuser.Id))
            {
                behaviours.Add(item);
            }

            if (behaviours.Count() == 0)
            {
                stat.MostListenedGenre = "-";
                stat.MinsSpent = 0;
                stat.MostListenedArtist = "-";
                stat.MostListenedSong = "-";
                stat.NumOfListenedGenre = 0;
                stat.Type = "Yearly";
            }
            else
            {

                //mostListenedGenre
                foreach (var item in behaviours)
                {
                    var music = _context.Musics.FirstOrDefault(x => x.Id == item.MusicId);
                    if (!genres.Contains(music.Genre))
                    {
                        genres.Add(music.Genre);
                        dict[music.Genre] = item.ListeningCount;
                    }
                    else
                    {
                        dict[music.Genre] += 1 * item.ListeningCount;
                    }
                }
                stat.MostListenedGenre = dict.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

                //minsSpent
                var minsSpent = 0;
                foreach (var item in behaviours)
                {
                    var music = _context.Musics.FirstOrDefault(x => x.Id == item.MusicId);
                    minsSpent += (int)Math.Round((StatMsToMins(music.DurationMs) * item.ListeningCount));
                }

                stat.MinsSpent = minsSpent;

                //mostListenedArtist
                var mostListenedArtist = from x in behaviours
                                         group x by x.Music.ArtistName into g
                                         select new KeyValuePair<string, double>
                                         (g.Key, g.Sum(y => y.ListeningCount * StatMsToMins(y.Music.DurationMs)));

                stat.MostListenedArtist = mostListenedArtist.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

                //mostListenedSong

                var mostListenedSong = from x in behaviours
                                       group x by x.Music.TrackName into g
                                       select new KeyValuePair<string, double>
                                       (g.Key, g.Sum(y => y.ListeningCount));

                stat.MostListenedSong = mostListenedSong.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

                //numOfListednedGenre

                stat.NumOfListenedGenre = dict.Count();
                stat.Type = "Yearly";
            }

            statstoReturn.Add(stat);

            return statstoReturn;
        }

        */
        public async Task<IEnumerable<string>> GetLast7Days(int id)
        {
            var givenuser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            var statstoReturn = new List<string>();

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            statstoReturn.Add(today.AddDays(-6).ToString());
            statstoReturn.Add(today.AddDays(-5).ToString());
            statstoReturn.Add(today.AddDays(-4).ToString());
            statstoReturn.Add(today.AddDays(-3).ToString());
            statstoReturn.Add(today.AddDays(-2).ToString());
            statstoReturn.Add(today.AddDays(-1).ToString());
            statstoReturn.Add(today.ToString());


            return statstoReturn;

        }
        public async Task<IEnumerable<int>> GetLast7DaysMins(int id)
        {
            var givenuser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            var statstoReturn = new List<int>();

            var behaviours = new List<UserBehavior>();

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            //kiszedjük az elmúlt egy hét (a mai naptól visszamenőleg 1 hétre)
            foreach (var item in _context.UserBehaviors.Where(x => x.Date > today.AddDays(-7) && x.UserId == givenuser.Id))
            {
                behaviours.Add(item);
            }


            statstoReturn.Add(GetMinutes(behaviours.Where(x => x.Date == today.AddDays(-6))));
            statstoReturn.Add(GetMinutes(behaviours.Where(x => x.Date == today.AddDays(-5))));
            statstoReturn.Add(GetMinutes(behaviours.Where(x => x.Date == today.AddDays(-4))));
            statstoReturn.Add(GetMinutes(behaviours.Where(x => x.Date == today.AddDays(-3))));
            statstoReturn.Add(GetMinutes(behaviours.Where(x => x.Date == today.AddDays(-2))));
            statstoReturn.Add(GetMinutes(behaviours.Where(x => x.Date == today.AddDays(-1))));
            statstoReturn.Add(GetMinutes(behaviours.Where(x => x.Date == today)));


            return statstoReturn;
        }

        #endregion

        #region actions

        public async Task<IEnumerable<Music>> AddSong(int id, string trackId)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            HttpClient httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.spotify.com/v1/tracks/{trackId}");
            request.Headers.Add("Authorization", $"Bearer {user.SpotifyAccessToken}");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.SpotifyAccessToken);

            var musics = new List<Music>();

            var response = await httpClient.GetAsync($"https://api.spotify.com/v1/audio-features/{trackId}");

            if (!response.IsSuccessStatusCode)
            {
                throw new SpotifyException();
            }
            else
            {
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();

                var audioFeatures = JsonConvert.DeserializeObject<SpotifyTrackDto>(responseContent);

                var httpClient2 = new HttpClient();
                httpClient2.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.SpotifyAccessToken);

                var response2 = await httpClient2.GetAsync($"https://api.spotify.com/v1/tracks/{trackId}");
                var responseContent2 = await response2.Content.ReadAsStringAsync();

                var track = JsonConvert.DeserializeObject<SpoitifyTrackMainFeaturesDto>(responseContent2);

                var artistId = track.Artists[0].Id;
                request = new HttpRequestMessage(HttpMethod.Get, $"https://api.spotify.com/v1/artists/{artistId}");
                request.Headers.Add("Authorization", $"Bearer {user.SpotifyAccessToken}");

                response = await httpClient.SendAsync(request);
                responseContent = await response.Content.ReadAsStringAsync();
                var artistInfo = JsonConvert.DeserializeObject<Artist>(responseContent);
                var musicToAdd = new Music();

                if (artistInfo.Genres.Length == 0)
                {
                    musicToAdd = new Music()
                    {
                        Genre = "Unspecified",
                        ArtistName = artistInfo.Name,
                        TrackName = track.Name,
                        TrackId = track.Id,
                        Popularity = track.Popularity,
                        Acousticness = audioFeatures.Acousticness,
                        Danceability = audioFeatures.Danceability,
                        DurationMs = audioFeatures.Duration_ms,
                        Energy = audioFeatures.Energy,
                        Key = audioFeatures.Key.ToString(),
                        Liveness = audioFeatures.Liveness,
                        Loudness = audioFeatures.Loudness,
                        Mode = ModeConverter(audioFeatures.Mode),
                        Speechiness = audioFeatures.Speechiness,
                        Tempo = audioFeatures.Tempo,
                        Valence = audioFeatures.Valence
                    };
                }
                else
                {
                    musicToAdd = new Music()
                    {
                        Genre = artistInfo.Genres[0],
                        ArtistName = artistInfo.Name,
                        TrackName = track.Name,
                        TrackId = track.Id,
                        Popularity = track.Popularity,
                        Acousticness = audioFeatures.Acousticness,
                        Danceability = audioFeatures.Danceability,
                        DurationMs = audioFeatures.Duration_ms,
                        Energy = audioFeatures.Energy,
                        Key = audioFeatures.Key.ToString(),
                        Liveness = audioFeatures.Liveness,
                        Loudness = audioFeatures.Loudness,
                        Mode = ModeConverter(audioFeatures.Mode),
                        Speechiness = audioFeatures.Speechiness,
                        Tempo = audioFeatures.Tempo,
                        Valence = audioFeatures.Valence
                    };

                }

                if (_context.Musics.FirstOrDefault(x => x.TrackId == musicToAdd.TrackId) == null)
                {
                    _context.Musics.Add(musicToAdd);
                    _context.SaveChanges();
                    musics.Add(musicToAdd);
                }
            }


            return musics;
        }

        public async Task<IEnumerable<Music>> AddSongWithListening(int id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.SpotifyAccessToken);

            var response = await httpClient.GetAsync("https://api.spotify.com/v1/me/player/currently-playing");
            if (!response.IsSuccessStatusCode)
            {
                throw new SpotifyException();
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var currentlyPlaying = JsonConvert.DeserializeObject<CurrentlyPlayingDto>(responseContent);
                string spotifyId = currentlyPlaying?.Item?.Id;

                //itt megvan az id, de még 3 kérés, hogy minden adatunk meglegyen, de ez ugyan az mint a másik hozzáadási metódus
                return await AddSong(user.Id, spotifyId);

            }
        }

        public async Task<UserBehavior> AddBehaviorWithListening(int id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.SpotifyAccessToken);

            var response = await httpClient.GetAsync("https://api.spotify.com/v1/me/player/currently-playing");

            if (!response.IsSuccessStatusCode)
            {
                throw new SpotifyException();
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var currentlyPlaying = JsonConvert.DeserializeObject<CurrentlyPlayingDto>(responseContent);
                string spotifyId = currentlyPlaying?.Item?.Id;

                /// ha nincs az adatbázisban, akkor először hozzáadjuk
                var m = _context.Musics.FirstOrDefault(x => x.TrackId == spotifyId);

                if (m == null) //hozzá kell adni
                {
                    if (spotifyId == null) //van amikor bugos az api hívás
                    {
                        return null;
                    }
                    var ms = await AddSong(user.Id, spotifyId);
                    //itt hozzá van már adva a zenékhez, márcsak a behaviorokhoz kell
                    m = _context.Musics.FirstOrDefault(x => x.TrackId == spotifyId);
                }
                var behavior = _context.UserBehaviors.FirstOrDefault(x => x.UserId == user.Id && x.MusicId == m.Id);

                if (behavior == null)
                {
                    behavior = new UserBehavior()
                    {
                        UserId = user.Id,
                        MusicId = m.Id,
                        Date = today,
                        ListeningCount = 1,
                        NameOfDay = DateTime.Now.DayOfWeek.ToString(),
                        TimeOfDay = TimeOfDayConverter()
                    };
                    _context.UserBehaviors.Add(behavior);
                }
                else
                {
                    behavior.ListeningCount++;
                    _context.UserBehaviors.Update(behavior);
                }
                _context.SaveChanges();

                return behavior;
            }
        }

        public async Task<UserBehavior> AddBehaviorWithButton(AccessTokenDTO dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == dto.userid);
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            var music = await _context.Musics.FirstOrDefaultAsync(x => x.TrackId == dto.token);
            var behav = await _context.UserBehaviors.FirstOrDefaultAsync(x => x.UserId == dto.userid && x.MusicId == music.Id);

            if (behav == null)
            {
                behav = new UserBehavior()
                {
                    UserId = user.Id,
                    MusicId = music.Id,
                    Date = today,
                    ListeningCount = 1,
                    NameOfDay = DateTime.Now.DayOfWeek.ToString(),
                    TimeOfDay = TimeOfDayConverter()
                };
                _context.UserBehaviors.Add(behav);

            }
            else
            {
                behav.ListeningCount++;
                _context.UserBehaviors.Update(behav);

            }
            _context.SaveChanges();

            return behav;
        }

        public async Task<PlaylistDto> CreateSpotifyPlaylist(AccessTokenDTO dto, List<string> mIds)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == dto.userid);
            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.SpotifyAccessToken);

            var timeOfDay = TimeOfDayConverter();
            var day = DateTime.Now.DayOfWeek.ToString();
            DateOnly date = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var playlistName = $"{day}, {timeOfDay} by Hingify";
            var desc = $"This playlist was created for @{user.UserName} by Hingify on {date}.";

            var requestContent = new StringContent(JsonConvert.SerializeObject(new { name = playlistName }));

            requestContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await httpClient.PostAsync($"https://api.spotify.com/v1/users/{user.SpotifyId}/playlists", requestContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new SpotifyException();
            }

            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();

            var playlist = JsonConvert.DeserializeObject<PlaylistDto>(responseContent);

            HttpClient httpClient2 = new HttpClient();
            httpClient2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.SpotifyAccessToken);

            var requestContent2 = new StringContent(JsonConvert.SerializeObject(new { uris = mIds }));
            requestContent2.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response2 = await httpClient2.PostAsync($"https://api.spotify.com/v1/playlists/{playlist.Id}/tracks", requestContent2);

            if (!response2.IsSuccessStatusCode)
            {
                throw new SpotifyException();
            }

            response2.EnsureSuccessStatusCode();

            return playlist;
        }

        #endregion

        #region mellekmetodusok

        private string ModeConverter(int num)
        {
            if (num == 1)
                return "Major";
            else
                return "Minor";
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

        public async Task<AppUser> GetUser(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }
        private int GetTimeLeftFromTimeOfDay(string timeOfDay)
        {
            int endHour = EndHour(timeOfDay);
            int currHour = DateTime.Now.Hour;
            int currMins = DateTime.Now.Minute;

            int hoursLeft = 0;
            int minsLeft = 0;

            if (currMins != 0)
                hoursLeft = endHour - currHour - 1;
            else
                hoursLeft = endHour - currHour;

            minsLeft = 60 - currMins;

            if (hoursLeft != 0)
                minsLeft = minsLeft + hoursLeft * 60;

            return minsLeft;
        }

        private bool LinkedListClusterContains(List<LinkedList> songCluster, Music otherMusic)
        {
            foreach (var cluster in songCluster)
            {
                LinkedListNode current = cluster.Head;
                while (current != null)
                {
                    if (current.Object.Equals(otherMusic))
                        return true;

                    current = current.Next;
                }
            }
            return false;
        }

        private bool LinkedListContains(LinkedList songsToChooseFrom, Music otherMusic)
        {
            LinkedListNode current = songsToChooseFrom.Head;
            while (current != null)
            {
                if (current.Object.Equals(otherMusic))
                    return true;

                current = current.Next;
            }
            return false;
        }

        private List<Music> GetSongsFromEveryGenre(List<LinkedList> songCluster)
        {
            var musics = new List<Music>();

            foreach (var cluster in songCluster)
            {
                LinkedListNode current = cluster.Head;
                if (current.Value == 0) //a hallgatott zenék ÉS plusz minden stílusból 1 zene belemegy
                {
                    musics.Add(current.Object);
                    if (current.Next != null)
                    {
                        current = current.Next;
                        musics.Add(current.Object);
                    }
                }

            }

            return musics;
        }

        private LinkedList MakePriorityQueueFromAllSongs(List<LinkedList> songCluster)
        {
            var musics = new LinkedList();

            foreach (var cluster in songCluster)
            {
                LinkedListNode current = cluster.Head;
                while (current != null)
                {
                    musics.Add(current.Value, current.Object);
                    current = current.Next;
                }
            }

            return musics;
        }

        private double GetAllLengthMixedGenre(List<LinkedList> songCluster)
        {
            double chooseableLength = 0;

            foreach (var cluster in songCluster)
            {
                LinkedListNode current = cluster.Head;
                while (current != null)
                {
                    chooseableLength += MsToMins(current.Object.DurationMs);
                    current = current.Next;
                }
            }

            return chooseableLength;
        }

        private double GetAllLengthOneGenre(LinkedList songsToChooseFrom)
        {
            double chooseableLength = 0;
            LinkedListNode current = songsToChooseFrom.Head;

            while (current != null)
            {
                chooseableLength += MsToMins(current.Object.DurationMs);
                current = current.Next;
            }

            return chooseableLength;
        }

        private void AddNewCluster(List<LinkedList> songCluster, Music listenedMusic, Music otherMusic)
        {
            songCluster.Add(new LinkedList()
            {
                Head = new LinkedListNode
                {
                    Object = otherMusic,
                    Value = EuclideanDistance(listenedMusic, otherMusic)
                }
            });
        }

        private void AddToCurrentCluster(List<LinkedList> songCluster, Music listenedMusic, Music otherMusic)
        {
            foreach (var item in songCluster)
            {
                if (item.Head.Object.Genre == otherMusic.Genre)
                {
                    item.Add(EuclideanDistance(listenedMusic, otherMusic), otherMusic);
                }
            }
        }

        private bool isGenreAlreadyACluster(List<LinkedList> songCluster, string genre)
        {
            foreach (var item in songCluster)
            {
                if (item.Head.Object.Genre == genre)
                {
                    return true;
                }
            }
            return false;
        }

        private bool isWithinRange(Music first, Music second)
        {
            if (EnergyInBourdaries(first.Energy, second.Energy) && ValenceInBourdaries(first.Valence, second.Valence) && AcousticnessInBourdaries(first.Acousticness, second.Acousticness))
                return true;

            return false;
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

        private Music GetMusic(UserBehavior behav)
        {
            return _context.Musics.FirstOrDefault(x => x.Id == behav.MusicId);
        }
        public IEnumerable<Music> GetMusics()
        {
            var query = from m in _context.Musics
                        orderby m.Id
                        select m;

            return query.ToList();
        }

        public IEnumerable<AppUser> GetUsers()
        {
            var query = from u in _context.Users
                        orderby u.Id
                        select u;

            return query.ToList();
        }

        public bool IsLiked(int userid, int musicid)
        {
            var likedsong = _context.LikedSongs.FirstOrDefault(x => x.UserId == userid && x.MusicId == musicid);

            if (_context.LikedSongs.Contains(likedsong))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddLikedSong(int userid, int musicid)
        {
            var givenuser = _context.Users.FirstOrDefault(x => x.Id == userid);

            _context.LikedSongs.Add(new LikedSong()
            {
                UserId = userid,
                MusicId = musicid
            });

            _context.SaveChanges();
        }

        public void RemoveFromLikedSong(int userid, int musicid)
        {
            var givenuser = _context.Users.FirstOrDefault(x => x.Id == userid);
            var toRemove = _context.LikedSongs.FirstOrDefault(x => x.UserId == userid && x.MusicId == musicid);

            _context.LikedSongs.Remove(toRemove);

            _context.SaveChanges();
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

            return genres;
        }
        private void RetrieveAccessToken(AccessTokenDTO accessToken)
        {
            var user = _context.Users.SingleOrDefault(x => x.Id == accessToken.userid);

            string authorizationCode = accessToken.token;
            string redirectUri = "http://localhost:4200/spotify-success";
            string clientId = _spotifySettings.ClientId;
            string clientSecret = _spotifySettings.ClientSecret;

            RestClient client = new RestClient("https://accounts.spotify.com");
            RestRequest request = new RestRequest("api/token", Method.Post);
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("code", authorizationCode);
            request.AddParameter("redirect_uri", redirectUri);
            request.AddParameter("client_id", clientId);
            request.AddParameter("client_secret", clientSecret);

            RestResponse response = client.Execute(request);
            string responseBody = response.Content;

            if (!responseBody.ToLower().Contains("error"))
            {
                var cut = JsonConvert.DeserializeObject<ResponseDTO>(responseBody);
                user.SpotifyAccessToken = cut.access_token;
                _context.SaveChanges();
            }
        }

        private async void RefreshToken(AccessTokenDTO accessToken)
        {
            string authorizationCode = accessToken.token;
            string redirectUri = "http://localhost:4200/foryou";
            string clientId = "1ec4eab22f26449491c0d514d9b464ef";
            string clientSecret = "ede6e9fc0b024434a1e9f6302f7873a4";
            ;
            var user = _context.Users.SingleOrDefault(x => x.Id == accessToken.userid);

            HttpClient httpClient2 = new HttpClient();

            // Set up the request body
            var requestContent2 = new FormUrlEncodedContent(new Dictionary<string, string>
            {
            {"grant_type", "authorization_code"},
            {"code", authorizationCode},
            {"redirect_uri", redirectUri},
            {"client_id", clientId},
            {"client_secret", clientSecret}
            });
            ;
            // Send the POST request to get the access token and refresh token
            var response2 = await httpClient2.PostAsync("https://accounts.spotify.com/api/token", requestContent2);
            ;
            response2.EnsureSuccessStatusCode();
            ;
            // Get the response content
            var responseContent2 = await response2.Content.ReadAsStringAsync();
            // Parse the JSON response to get the new access token
            var tokenResponse2 = JsonConvert.DeserializeObject<ResponseDTO>(responseContent2);
            ;

            user.SpotifyAccessToken = tokenResponse2.access_token;
            _context.SaveChanges();
        }

        private bool ContainsMusicInDB(string? trackId)
        {
            if (_context.Musics.FirstOrDefault(x => x.TrackId == trackId) != null)
                return true;
            else
                return false;
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

            return (int)(conv * ms);
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
        private bool ContainsLikedSong(int id, List<LikedSong> songs)
        {
            foreach (var s in songs)
            {
                if (id == s.MusicId)
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

        private double StatMsToMins(int ms)
        {
            double conv = 1.6667E-5;
            var x = (double)conv * ms;

            return (double)conv * ms;
        }

        private int GetMinutes(IEnumerable<UserBehavior> behavs)
        {
            int mins = 0;

            foreach (var item in behavs)
            {
                var music = _context.Musics.FirstOrDefault(x => x.Id == item.MusicId);
                mins += (int)Math.Round((StatMsToMins(music.DurationMs) * item.ListeningCount));
            }


            return mins;
        }

        #endregion

    }
}
