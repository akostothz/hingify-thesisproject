using System.Security.Cryptography;
using System.Text;
using UNN1N9_SOF_2022231_BACKEND.Data;
using UNN1N9_SOF_2022231_BACKEND.DTOs;
using UNN1N9_SOF_2022231_BACKEND.Models;

namespace UNN1N9_SOF_2022231_BACKEND.Seeds
{
    public static class DataSeeder
    {
        public static void Seed(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            context.Database.EnsureCreated();
            AddMusics(context);
            //AddTestUsers(context);
            //AddTestConnections(context);
        }

        private static string ReformatCsv(string csvfile)
        {

        }

        private static void AddMusics(DataContext context)
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
        private static void AddTestUsers(DataContext context)
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
        }
        private static void AddTestConnections(DataContext context)
        {
            //EZEKET UNIT TESZTBE KISZERVEZNI!!

            //pali > 2 R & B zene, azaz a férfiaknak, a magyaroknak,
            //és a 19 - 25 éves korosztályban ezeket kéne bedobni
            var pali = context.Users.FirstOrDefault(x => x.Id == 3);
            var paliMusic1 = context.Musics.FirstOrDefault(y => y.Id == 119);
            var paliMusic2 = context.Musics.FirstOrDefault(y => y.Id == 125);

            context.UserBehaviors.Add(new Models.UserBehavior()
            {
                UserId = 3,
                MusicId = 119,
                ListeningCount = 3,
                NameOfDay = "Monday",
                TimeOfDay = "Evening"
            });
            context.UserBehaviors.Add(new Models.UserBehavior()
            {
                UserId = 3,
                MusicId = 125,
                ListeningCount = 4,
                NameOfDay = "Monday",
                TimeOfDay = "Evening"
            });

            //adel > 2 alternatív zene, azaz a nőknek, a franciáknak 
            //és a 26-39 éves korosztályban ezeket kéne bedobni
            var adel = context.Users.FirstOrDefault(x => x.Id == 6);
            var adelMusic1 = context.Musics.FirstOrDefault(y => y.Id == 388);
            var adelMusic2 = context.Musics.FirstOrDefault(y => y.Id == 385);

            context.UserBehaviors.Add(new Models.UserBehavior()
            {
                UserId = 6,
                MusicId = 388,
                ListeningCount = 5,
                NameOfDay = "Friday",
                TimeOfDay = "Morning"
            });
            context.UserBehaviors.Add(new Models.UserBehavior()
            {
                UserId = 6,
                MusicId = 385,
                ListeningCount = 9,
                NameOfDay = "Friday",
                TimeOfDay = "Forenoon"
            });

            var ozil = context.Users.FirstOrDefault(x => x.Id == 5);
            var ozilMusic1 = context.Musics.FirstOrDefault(y => y.Id == 1538);
            var ozilMusic2 = context.Musics.FirstOrDefault(y => y.Id == 1571);

            context.UserBehaviors.Add(new Models.UserBehavior()
            {
                UserId = 5,
                MusicId = 1538,
                ListeningCount = 5,
                NameOfDay = "Friday",
                TimeOfDay = "Morning"
            });
            context.UserBehaviors.Add(new Models.UserBehavior()
            {
                UserId = 5,
                MusicId = 1571,
                ListeningCount = 9,
                NameOfDay = "Friday",
                TimeOfDay = "Forenoon"
            });

            context.SaveChanges();
        }
    }
}
