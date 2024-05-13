using Microsoft.EntityFrameworkCore.Internal;
using SpotifyAPI.Web;
using System;
using System.Security.Cryptography;
using System.Text;
using UNN1N9_SOF_2022231_BACKEND.Data;
using UNN1N9_SOF_2022231_BACKEND.DTOs;
using UNN1N9_SOF_2022231_BACKEND.Helpers;
using UNN1N9_SOF_2022231_BACKEND.Interfaces;
using UNN1N9_SOF_2022231_BACKEND.Models;
using UNN1N9_SOF_2022231_BACKEND.Services;
using static SpotifyAPI.Web.SearchRequest;

namespace UNN1N9_SOF_2022231_BACKEND.Seeds
{
    public static class DataSeeder
    {
        public static void Seed(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            context.Database.EnsureCreated();

            // metódusok meghívása
            AddMusics(context);
            AddTestUsers(context);
            AddMinutesForStats(context);    
        }

        #region DBfeloltese

        private static void AddMinutesForStats(DataContext context)                             //3. id-jű user mindig kap random Behavior-okat, hogy a statisztika szép legyen
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            for (int i = 0; i >= -7; i--)
            {
                context.UserBehaviors.Add(new UserBehavior { MusicId = RandomGenerator.rnd.Next(50000), UserId = 3, ListeningCount = RandomGenerator.rnd.Next(5), Date = today.AddDays(i), NameOfDay = "Monday", TimeOfDay = "Night" });
            }

            context.SaveChanges();
        }

        private static void AddLikedSongs(DataContext context)                                  //véletlenszerű kedvelt dalok hozzáadása
        {
            foreach (var item in context.Users)
            {
                int numberToAdd = RandomGenerator.rnd.Next(15, 30);
                for (int i = 0; i < numberToAdd; i++)
                {
                    context.LikedSongs.Add(new LikedSong() { UserId = item.Id, MusicId = RandomGenerator.rnd.Next(1, 220000) });
                }
            }

            context.SaveChanges();
        }

        private static void AddMusics(DataContext context)                                      //zenék hozzáadása, ha még nincs feltöltve
        {
            var music = context.Musics.FirstOrDefault();
            if (music == null)
            {
                using (var reader = new StreamReader(@"./bin/Debug/net6.0/SubduedSpotifyDatabase.csv"))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(';');
                        if (values.Length == 17)
                        {
                            context.Musics.Add(new Models.Music()
                            {
                                Genre = values[1],
                                ArtistName = values[2],
                                TrackName = values[3],
                                TrackId = values[4],
                                Popularity = int.Parse(values[5]),
                                Acousticness = double.Parse(values[6]),
                                Danceability = double.Parse(values[7]),
                                DurationMs = int.Parse(values[8]),
                                Energy = double.Parse(values[9]),
                                Key = values[10],
                                Liveness = double.Parse(values[11]),
                                Loudness = double.Parse(values[12]),
                                Mode = values[13],
                                Speechiness = double.Parse(values[14]),
                                Tempo = double.Parse(values[15]),
                                Valence = double.Parse(values[16])
                            });
                        }
                    }
                }
            }
            context.SaveChanges();
        }
        private static void AddTestUsers(DataContext context)                                   //felhasználók hozzáadása, ha még nem léteznek
        {
            var users = context.Users.FirstOrDefault();
            if (users == null)
            {
                using var hmac = new HMACSHA512();

                context.Users.Add(new Models.AppUser()
                {
                    Email = "pali@pali.com",
                    UserName = "pali",
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Almafa123")),
                    PasswordSalt = hmac.Key,
                    FirstName = "Pal",
                    LastName = "Toth",
                    YearOfBirth = 2001,
                    Country = "Hungary",
                    Gender = "Male"
                });
                context.Users.Add(new Models.AppUser()
                {
                    Email = "panni@panni.com",
                    UserName = "panni",
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Almafa123")),
                    PasswordSalt = hmac.Key,
                    FirstName = "Panna",
                    LastName = "Kis",
                    YearOfBirth = 1974,
                    Country = "Hungary",
                    Gender = "Female"
                });
                context.Users.Add(new Models.AppUser()
                {
                    Email = "ozil@ozil.com",
                    UserName = "m.ozil",
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Almafa123")),
                    PasswordSalt = hmac.Key,
                    FirstName = "Mesut",
                    LastName = "Ozil",
                    YearOfBirth = 1982,
                    Country = "Germany",
                    Gender = "Male"
                });
                context.Users.Add(new Models.AppUser()
                {
                    Email = "adel@adel.com",
                    UserName = "leda",
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Almafa123")),
                    PasswordSalt = hmac.Key,
                    FirstName = "Adel",
                    LastName = "Brull",
                    YearOfBirth = 1994,
                    Country = "France",
                    Gender = "Female"
                });

                context.SaveChanges();

                AddLikedSongs(context);
            }

        }

        #endregion

        #region teszteleshez
        private static void Choose100Songs(DataContext context)                                 //100 véletlenszerű dal átlagát kiszámító metódus
        {
            string[] styles = { "Rock", "Electronic", "Alternative", "Country", "Pop", "R&B", "Hip-Hop", "Jazz", "Rap", "Dance", "Classical", "Indie" };
            string genre = "";
            string output = "";

            for (int i = 0; i < styles.Length; i++)
            {
                genre = styles[i];
                output += $"Genre: {genre}\n\n";
                int totalMusicCount = context.Musics
                .Where(x => x.Genre.Equals(genre))
                .Count();
                double sum = 0;

                for (int j = 0; j < 100; j++)
                {
                    int randomIndex = RandomGenerator.rnd.Next(totalMusicCount);
                    var song = context.Musics
                        .Where(x => x.Genre.Equals(genre))
                        .OrderBy(m => m.Id)
                        .Skip(randomIndex)
                        .FirstOrDefault();

                    Console.WriteLine(song.ArtistName + ": " + song.TrackName);
                    output += $"{song.ArtistName}: {song.TrackName}\n";
                    sum += FindMore(context, song.TrackId, ref output);

                }
                output += $"\n\nMean of the 100 songs >>> {Math.Round((sum / 100), 4)}\n\n";

                output += $"-----------------------------------------------\n\n\n";
            }

            using (StreamWriter writer = new StreamWriter("100songs.txt", true))
            {
                writer.WriteLine(output);
            }

        }

        private static void WriteMockObjects(DataContext context)                               //unit teszt Mock objektumjait íratja ki
        {
            var songs = new List<Music>();
            var genre = "Electronic";
            int totalMusicCount = context.Musics.Where(x => x.Genre.Equals(genre) && x.Mode.Equals("Minor")).Count();

            for (int i = 0; i < 5; i++)
            {
                int randomIndex = RandomGenerator.rnd.Next(totalMusicCount);
                var song = context.Musics.Where(x => x.Genre.Equals(genre) && x.Mode.Equals("Minor")).OrderBy(m => m.Id).Skip(randomIndex).FirstOrDefault();
                songs.Add(song);
            }
            int id = 15;
            foreach (var song in songs)
            {
                Console.WriteLine($"new Music {{ Id = {id++}, Genre = \"{song.Genre}\", ArtistName = \"{song.ArtistName}\", TrackName = \"{song.TrackName}\", TrackId = \"{song.TrackId}\", Acousticness = {song.Acousticness.ToString().Replace(',', '.')}, Danceability = {song.Danceability.ToString().Replace(',', '.')}, DurationMs = {song.DurationMs}, Energy = {song.Energy.ToString().Replace(',', '.')}, Key = \"{song.Key}\", Liveness = {song.Liveness.ToString().Replace(',', '.')}, Loudness = {song.Loudness.ToString().Replace(',', '.')}, Mode = \"{song.Mode}\", Speechiness = {song.Speechiness.ToString().Replace(',', '.')}, Tempo = {song.Tempo.ToString().Replace(',', '.')}, Valence = {song.Valence.ToString().Replace(',', '.')} }},");
            }
        }

        private static void CalculatePairs(DataContext context, string filePath)                //saját és véletlenszerű stílusokból randokm ajánlások
        {
            string[] pairs = {
                "Rock;Rock",
                "Rock;Electronic",
                "Electronic;Electronic",
                "Alternative;Alternative",
                "Alternative;Country",
                "Country;Country",
                "Pop;Pop",
                "Pop;R&B",
                "R&B;R&B",
                "Hip-Hop;Hip-Hop",
                "Hip-Hop;Jazz",
                "Jazz;Jazz",
                "Rap;Rap",
                "Rap;Dance",
                "Dance;Dance",
                "Classical;Classical",
                "Classical;Indie",
                "Indie;Indie"
            };

            int totalMusicCount = 0;
            int totalMusicCount2 = 0;

            for (int i = 0; i < pairs.Length; i++)
            {
                string s1 = pairs[i].Split(';')[0];
                string s2 = pairs[i].Split(';')[1];

                totalMusicCount = context.Musics
                .Where(x => x.Genre.Equals(s1))
                .Count();
                totalMusicCount2 = context.Musics
                    .Where(x => x.Genre.Equals(s2))
                    .Count();

                int randomIndex = RandomGenerator.rnd.Next(totalMusicCount);
                int randomIndex2 = RandomGenerator.rnd.Next(totalMusicCount2);

                //kiválasztjuk a random zenét hozzá
                var song = context.Musics
                    .Where(x => x.Genre.Equals(s1))
                    .OrderBy(m => m.Id)
                    .Skip(randomIndex)
                    .FirstOrDefault();


                var otherGenre = context.Musics
                        .Where(x => x.Genre.Equals(s2))
                        .OrderBy(m => m.Id)
                        .Skip(randomIndex2)
                        .FirstOrDefault();


                // 2 ugyan az euklideszi távolsága

                double other = EuclideanDistance(song, otherGenre);
                string output = "";
                output += "-----------------------------------------\n";
                output += $"FIRST SONG :: {song.TrackName} by {song.ArtistName} ({song.Genre})\n";
                output += $"SECOND SONG :: {otherGenre.TrackName} by {otherGenre.ArtistName} ({otherGenre.Genre})\n\n\n";
                output += $"[[[{song.Genre} - {otherGenre.Genre}]]] >>> {Math.Round(other, 4)}\n";
                output += "-----------------------------------------\n";


                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine(output);
                }

            }

        }
        private static void ChooseRandomSongs(DataContext context)                              //random zenék kiválogatása adott stílusból
        {
            var songs = new List<Music>();

            var genre = "Rock";
            int totalMusicCount = context.Musics
                .Where(x => x.Genre.Equals(genre))
                .Count();

            for (int i = 0; i <= 10; i++)
            {
                int randomIndex = RandomGenerator.rnd.Next(totalMusicCount);
                var song = context.Musics
                    .Where(x => x.Genre.Equals(genre))
                    .OrderBy(m => m.Id)
                    .Skip(randomIndex)
                    .FirstOrDefault();
                songs.Add(song);
            }

            foreach (var song in songs)
            {
                Console.WriteLine(song.ArtistName + ": " + song.TrackName);
            }
        }
        private static double FindMore(DataContext _context, string trackId, ref string output) //legközelebbi zenék kiválogatása
        {
            var choosenMusic = _context.Musics.FirstOrDefault(x => x.TrackId == trackId);
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

            return Get5ClosestSongs(closestMusics, ref output); //csak a tesztelés miatt van

        }

        private static double Get5ClosestSongs(LinkedList closestMusics, ref string output)     //5 legközelebbi dal kiválasztása
        {
            LinkedListNode current = closestMusics.Head;
            double[] distances = new double[6];
            int counter = 0;
            while (current != null)
            {
                //Console.WriteLine(current.Object.ArtistName + ": " + current.Object.TrackName + " >>> " + Math.Round(current.Value, 4));
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
            output += $"Mean: {Math.Round(x, 4)}\n";

            return x;
        }

        #endregion    

        #region mellekmetodusok

        private static double EuclideanDistance(Music currMusic, Music dbMusic)                 //euklidészi távolság
        {
            double dist = Math.Sqrt(Math.Pow(currMusic.Energy - dbMusic.Energy, 2) +
                                Math.Pow(currMusic.Valence - dbMusic.Valence, 2) +
                                Math.Pow(currMusic.Acousticness - dbMusic.Acousticness, 2));
            return dist;
        }

        private static bool EverythingInBoundaries(Music music, Music? choosenMusic)
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

        private static bool FMEnergyInBourdaries(double currEnergy, double dbEnergy)
        {
            if (dbEnergy <= currEnergy + 0.15 && dbEnergy >= currEnergy - 0.15)
                return true;

            return false;
        }

        private static bool FMValenceInBourdaries(double currValence, double dbValence)
        {
            if (dbValence <= currValence + 0.15 && dbValence >= currValence - 0.15)
                return true;

            return false;
        }

        private static bool FMAcousticnessInBourdaries(double currAcousticness, double dbAcousticness)
        {
            if (dbAcousticness <= currAcousticness + 0.15 && dbAcousticness >= currAcousticness - 0.15)
                return true;

            return false;
        }

        private static bool FMTempoInBoundaries(double currTempo, double dbTempo)
        {
            if (dbTempo <= currTempo + 8 && dbTempo >= currTempo - 8)
                return true;

            return false;
        }

        private static bool FMSpeechinessInBoundaries(double currSpeechiness, double dbSpeechiness)
        {
            if (dbSpeechiness <= currSpeechiness + 0.15 && dbSpeechiness >= currSpeechiness - 0.15)
                return true;

            return false;
        }

        private static bool FMDanceabilityInBourdaries(double currDanceability, double dbDanceability)
        {
            if (dbDanceability <= currDanceability + 0.15 && dbDanceability >= currDanceability - 0.15)
                return true;

            return false;
        }

        #endregion
    }
}
