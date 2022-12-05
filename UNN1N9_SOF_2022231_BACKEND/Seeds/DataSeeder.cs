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
            //4 felhasználó a teszteléshez - ország, korosztály és nem szerinti ajánláshoz
            /*
             korosztályok:
              1-11
              12-17
              18-25
              26-39
              40-60
              60+

             nemek:
              férfi
              nő
              egyéb
             */


            /*  a tesztajánlások:
               pali és panni >>> régió alapján (Hungary)
               panni és m.ozil >>> Korcsoport alapján (40-60)
               panni es leda >>> Nem alapján (Female)
             */
            context.Users.Add(new Models.AppUser()
            {
                Email = "pali@pali.com",
                UserName = "pali",
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Almafa123")),
                PasswordSalt = hmac.Key,
                FirstName = "Pal",
                LastName = "Toth",
                Age = 22,
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
                Age = 49,
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
                Age = 41,
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
                Age = 29,
                Country = "France",
                Gender = "Female"
            });

            context.SaveChanges();
        }
        private static void AddTestConnections(DataContext context)
        {
            //néhány zenét hozzáadni a tesztajánlásokhoz
            //nap, napszak valamint nap és napszak alapján is lehetne ajánlani más felhasználóktól

            //napok: Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday

            /*
            napszakok:
              
            hajnal (Dawn > 0h - 5h)
            reggel (Morning > 5h - 9h)
            délelőtt (Forenoon > 9h - 12h)
            délután (Afternoon > 12h - 17h)
            este (Evening > 17h > 21h)
            éjszaka (Night 21h > 0h)
            */

            //pali > 2 R&B zene, azaz a férfiaknak, a magyaroknak,
            //és a 19-25 éves korosztályban ezeket kéne bedobni
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
                ListeningCount = 3,
                NameOfDay = "Monday",
                TimeOfDay = "Evening"
            });

            //adel > 2 alternatív zene, azaz a nőknek, a franciáknak 
            //és a 26-39 éves korosztályban ezeket kéne bedobni
            context.UserBehaviors.Add(new Models.UserBehavior()
            {
                UserId = 6,
                MusicId = 388,
                ListeningCount = 3,
                NameOfDay = "Friday",
                TimeOfDay = "Morning"
            });
            context.UserBehaviors.Add(new Models.UserBehavior()
            {
                UserId = 6,
                MusicId = 385,
                ListeningCount = 3,
                NameOfDay = "Friday",
                TimeOfDay = "Forenoon"
            });

            context.SaveChanges();
        }
    }
}
