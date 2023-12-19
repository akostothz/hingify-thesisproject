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

namespace UNN1N9_SOF_2022231_BACKEND.Seeds
{
    public static class DataSeeder
    {
        public static void Seed(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            context.Database.EnsureCreated();
            ChooseRandomSongs(context);
            //AddMusics(context);
            //AddTestUsers(context);
            //AddTestConnections(context);
            //AddFullDb(context);
            //FixConnectionDates(context);
            //AddTestConnectionForAkos(context);
            //AddLikedSongs(context);
            //AddUserBehaviors(context);
            //AddUserBehaviorsFor345ID(context);
        }

        private static void ChooseRandomSongs(DataContext context)
        {
            var songs = new List<Music>();
            var genre = "Country";
            int totalMusicCount = context.Musics.Where(x => x.Genre.Equals(genre)).Count();          

            for (int i = 0; i < 15; i++)
            {
                int randomIndex = RandomGenerator.rnd.Next(totalMusicCount);
                var song = context.Musics.Where(x => x.Genre.Equals(genre)).OrderBy(m => m.Id).Skip(randomIndex).FirstOrDefault();
                songs.Add(song);
            }

            foreach (var song in songs)
            {
                Console.WriteLine(song.ArtistName + ": " + song.TrackName);
            }
        }



        private static void AddUserBehaviorsFor345ID(DataContext context)
        {
            //először kitörlöm az összes behaviort, ami hozzájuk tartozik
            foreach (var item in context.UserBehaviors)
            {
                if (item.UserId == 3)
                {
                    context.UserBehaviors.Remove(item);
                }
                if (item.UserId == 4)
                {
                    context.UserBehaviors.Remove(item);
                }
                if (item.UserId == 5)
                {
                    context.UserBehaviors.Remove(item);
                }
            }

            context.SaveChanges();

            // majd feltöltöm statikusan (az egysezerűség kedvéért minden nap és napszakhoz ugyan
            // azokat a zenéket adom hozzá egy usernél - (6/stílus) db

            // id == 3 -> csak Alternatív zenék >> 6db
            // id == 4 -> Rock és Rap zenék >> 3db - 3db
            // id == 5 -> Dance, Electronic és Hip-Hop zenék >> 2db - 2db - 2db

            foreach (var item in context.Users)
            {
                if (item.Id == 3 || item.Id == 4 || item.Id == 5)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        for (int j = 0; j < 6; j++)
                        {
                            string day = GetDay(i);
                            string timeofDay = GetTimeOfDay(j);
                            int listeningCount = RandomGenerator.rnd.Next(1, 6);
                            DateOnly date;
                            if (day == "Monday")
                            {
                                date = new DateOnly(2023, 5, 1);
                            }
                            else if (day == "Tuesday")
                            {
                                date = new DateOnly(2023, 5, 2);
                            }
                            else if (day == "Wednesday")
                            {
                                date = new DateOnly(2023, 5, 3);
                            }
                            else if (day == "Thursday")
                            {
                                date = new DateOnly(2023, 5, 4);
                            }
                            else if (day == "Friday")
                            {
                                date = new DateOnly(2023, 5, 5);
                            }
                            else if (day == "Saturday")
                            {
                                date = new DateOnly(2023, 5, 6);
                            }
                            else
                            {
                                date = new DateOnly(2023, 5, 7);
                            }


                            if (item.Id == 3)
                            {
                                //a 6db alternatív zene id-ja: 358, 360, 361, 366, 380, 394
                                context.UserBehaviors.Add(new UserBehavior()
                                {
                                    UserId = 3,
                                    MusicId = 358,
                                    ListeningCount = listeningCount,
                                    NameOfDay = day,
                                    TimeOfDay = timeofDay,
                                    Date = date
                                });
                                context.UserBehaviors.Add(new UserBehavior()
                                {
                                    UserId = 3,
                                    MusicId = 360,
                                    ListeningCount = listeningCount,
                                    NameOfDay = day,
                                    TimeOfDay = timeofDay,
                                    Date = date
                                });
                                context.UserBehaviors.Add(new UserBehavior()
                                {
                                    UserId = 3,
                                    MusicId = 361,
                                    ListeningCount = listeningCount,
                                    NameOfDay = day,
                                    TimeOfDay = timeofDay,
                                    Date = date
                                });
                                context.UserBehaviors.Add(new UserBehavior()
                                {
                                    UserId = 3,
                                    MusicId = 366,
                                    ListeningCount = listeningCount,
                                    NameOfDay = day,
                                    TimeOfDay = timeofDay,
                                    Date = date
                                });
                                context.UserBehaviors.Add(new UserBehavior()
                                {
                                    UserId = 3,
                                    MusicId = 380,
                                    ListeningCount = listeningCount,
                                    NameOfDay = day,
                                    TimeOfDay = timeofDay,
                                    Date = date
                                });
                                context.UserBehaviors.Add(new UserBehavior()
                                {
                                    UserId = 3,
                                    MusicId = 394,
                                    ListeningCount = listeningCount,
                                    NameOfDay = day,
                                    TimeOfDay = timeofDay,
                                    Date = date
                                });

                            }
                            if (item.Id == 4)
                            {
                                //3db rock: 153000, 153004, 153193
                                //3db Rap: 115991, 116031, 116047

                                context.UserBehaviors.Add(new UserBehavior()
                                {
                                    UserId = 4,
                                    MusicId = 153000,
                                    ListeningCount = listeningCount,
                                    NameOfDay = day,
                                    TimeOfDay = timeofDay,
                                    Date = date
                                });
                                context.UserBehaviors.Add(new UserBehavior()
                                {
                                    UserId = 4,
                                    MusicId = 153004,
                                    ListeningCount = listeningCount,
                                    NameOfDay = day,
                                    TimeOfDay = timeofDay,
                                    Date = date
                                });
                                context.UserBehaviors.Add(new UserBehavior()
                                {
                                    UserId = 4,
                                    MusicId = 153193,
                                    ListeningCount = listeningCount,
                                    NameOfDay = day,
                                    TimeOfDay = timeofDay,
                                    Date = date
                                });
                                context.UserBehaviors.Add(new UserBehavior()
                                {
                                    UserId = 4,
                                    MusicId = 115991,
                                    ListeningCount = listeningCount,
                                    NameOfDay = day,
                                    TimeOfDay = timeofDay,
                                    Date = date
                                });
                                context.UserBehaviors.Add(new UserBehavior()
                                {
                                    UserId = 4,
                                    MusicId = 116031,
                                    ListeningCount = listeningCount,
                                    NameOfDay = day,
                                    TimeOfDay = timeofDay,
                                    Date = date
                                });
                                context.UserBehaviors.Add(new UserBehavior()
                                {
                                    UserId = 4,
                                    MusicId = 116047,
                                    ListeningCount = listeningCount,
                                    NameOfDay = day,
                                    TimeOfDay = timeofDay,
                                    Date = date
                                });
                            }
                            if (item.Id == 5)
                            {
                                //2db dance: 548, 592
                                //2db electronic: 693, 700
                                //2db hip-hop: 71423, 71425 

                                context.UserBehaviors.Add(new UserBehavior()
                                {
                                    UserId = 5,
                                    MusicId = 548,
                                    ListeningCount = listeningCount,
                                    NameOfDay = day,
                                    TimeOfDay = timeofDay,
                                    Date = date
                                });
                                context.UserBehaviors.Add(new UserBehavior()
                                {
                                    UserId = 5,
                                    MusicId = 592,
                                    ListeningCount = listeningCount,
                                    NameOfDay = day,
                                    TimeOfDay = timeofDay,
                                    Date = date
                                });
                                context.UserBehaviors.Add(new UserBehavior()
                                {
                                    UserId = 5,
                                    MusicId = 693,
                                    ListeningCount = listeningCount,
                                    NameOfDay = day,
                                    TimeOfDay = timeofDay,
                                    Date = date
                                });
                                context.UserBehaviors.Add(new UserBehavior()
                                {
                                    UserId = 5,
                                    MusicId = 700,
                                    ListeningCount = listeningCount,
                                    NameOfDay = day,
                                    TimeOfDay = timeofDay,
                                    Date = date
                                });
                                context.UserBehaviors.Add(new UserBehavior()
                                {
                                    UserId = 5,
                                    MusicId = 71423,
                                    ListeningCount = listeningCount,
                                    NameOfDay = day,
                                    TimeOfDay = timeofDay,
                                    Date = date
                                });
                                context.UserBehaviors.Add(new UserBehavior()
                                {
                                    UserId = 5,
                                    MusicId = 71425,
                                    ListeningCount = listeningCount,
                                    NameOfDay = day,
                                    TimeOfDay = timeofDay,
                                    Date = date
                                });
                            }
                        }
                    }
                }
                            
               
            }

            context.SaveChanges();
        }

        private static void AddUserBehaviors(DataContext context)
        {
            // a 3-as és 4-es és 5-ös id-jú egyedeket szándékosan kezelem külön és töltöm fel más számokkal,
            // nekik 1, 2 illetve 3 fajta stílust szeretnék csak megadni a teszt érdekében. 
            // a többiek pedig vegyesek lesznek, remélhetőleg 3-nál több fajta stílussal egy napszakban

            foreach (var item in context.Users)
            {
                if (item.Id != 3 || item.Id != 4 || item.Id != 5)
                {
                    for (int i = 0; i < 7; i++) //minden nap
                    {
                        string day = GetDay(i);
                        for (int j = 0; j < 6; j++) //minden napszakjába rakok Behaviorok-et
                        {
                            int numberToAdd = RandomGenerator.rnd.Next(6, 12); //de csak 6-11 db-ot
                            for (int k = 0; k < numberToAdd; k++)
                            {
                                string timeofDay = GetTimeOfDay(j);
                                int listeningCount = RandomGenerator.rnd.Next(1, 6);
                                DateOnly date;
                                if (day == "Monday")
                                {
                                    date = new DateOnly(2023, 5, 1);
                                }
                                else if (day == "Tuesday")
                                {
                                    date = new DateOnly(2023, 5, 2);
                                }
                                else if (day == "Wednesday")
                                {
                                   date = new DateOnly(2023, 5, 3);
                                }
                                else if (day == "Thursday")
                                {
                                    date = new DateOnly(2023, 5, 4);

                                }
                                else if (day == "Friday")
                                {
                                    date = new DateOnly(2023, 5, 5);
                                }
                                else if (day == "Saturday")
                                {
                                    date = new DateOnly(2023, 5, 6);
                                }
                                else
                                {
                                    date = new DateOnly(2023, 5, 7);
                                }

                                context.UserBehaviors.Add(new UserBehavior()
                                {
                                    UserId = item.Id,
                                    MusicId = RandomGenerator.rnd.Next(1, 220000),
                                    ListeningCount = listeningCount,
                                    NameOfDay = day,
                                    TimeOfDay = timeofDay,
                                    Date = date
                                });
                            }                         
                        }
                    }
                    
                }     
            }
            context.SaveChanges();
        }
        private static string GetTimeOfDay(int j)
        {
            switch (j)
            {
                case 0:
                    return "Dawn";
                    break;
                case 1:
                    return "Morning";
                    break;
                case 2:
                    return "Forenoon";
                    break;
                case 3:
                    return "Afternoon";
                    break;
                case 4:
                    return "Evening";
                    break;
                case 5:
                    return "Night";
                    break;
                default:
                    return "Forenoon";
                    break;
            }
        }
        private static string GetDay(int i)
        {
            switch (i)
            {
                case 0: 
                    return "Monday";
                    break;
                case 1:
                    return "Tuesday";
                    break;
                case 2:
                    return "Wednesday";
                    break;
                case 3:
                    return "Thursday";
                    break;
                case 4:
                    return "Friday";
                    break;
                case 5:
                    return "Saturday";
                    break;
                case 6:
                    return "Sunday";
                    break;
                default:
                    return "Saturday";
                    break;
            }
        }

        private static void AddLikedSongs(DataContext context)
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

        private static void AddTestConnectionForAkos(DataContext context)
        {
            foreach (var item in context.UserBehaviors.Where(x => x.UserId == 3))
            {
                if (RandomGenerator.rnd.Next(1, 3) == 1)
                {
                    item.Date = new DateOnly(2023, 5, RandomGenerator.rnd.Next(1, 12));
                }
            }
            context.SaveChanges();
        }

        private static void FixConnectionDates(DataContext context)
        {
            foreach (var item in context.UserBehaviors)
            {
                item.Date = new DateOnly(2023, RandomGenerator.rnd.Next(1, 5), RandomGenerator.rnd.Next(1, 28));
            }

            context.SaveChanges();
        }

        private static void AddFullDb(DataContext context)
        {
            using (var reader = new StreamReader(@"./bin/Debug/net6.0/SpotifyDatabase.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');
                    if (values.Length == 18)
                    {
                        string actualTrackId = values[3];
                        var duplicated = context.Musics.FirstOrDefault(x => x.TrackId == actualTrackId);
                        if (duplicated == null)
                        {
                            if (!NoMissingValues(values))
                            {
                                context.Musics.Add(new Models.Music()
                                {
                                    Genre = values[0],
                                    ArtistName = values[1],
                                    TrackName = values[2],
                                    TrackId = values[3],
                                    Popularity = int.Parse(values[4]),
                                    Acousticness = ReformatValue(values[5]),
                                    Danceability = ReformatValue(values[6]),
                                    DurationMs = int.Parse(values[7]),
                                    Energy = ReformatValue(values[8]),
                                    Key = values[10],
                                    Liveness = ReformatValue(values[11]),
                                    Loudness = ReformatValue(values[12]),
                                    Mode = values[13],
                                    Speechiness = ReformatValue(values[14]),
                                    Tempo = ReformatValue(values[15]),
                                    Valence = ReformatValue(values[17])
                                });
                            }
                        }
                    }
                }
            }
            context.SaveChanges();
        }

        private static bool NoMissingValues(string[] values)
        {
            bool IsThereMissinValue = false;

            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] == "" || values[i] == null || values[i] == String.Empty)
                {
                    IsThereMissinValue = true;
                }
            }

            return IsThereMissinValue;
        }

        private static double ReformatValue(string stringValue)
        {
            string c = stringValue.Replace('.', ',');
            double value = double.Parse(c);

            return value;
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

            context.LikedSongs.Add(new Models.LikedSong()
            {
                UserId = 3,
                MusicId = 119
            });
            context.LikedSongs.Add(new Models.LikedSong()
            {
                UserId = 3,
                MusicId = 125
            });

            //adel > 2 alternatív zene, azaz a nőknek, a franciáknak 
            //és a 26-39 éves korosztályban ezeket kéne bedobni
            //var adel = context.Users.FirstOrDefault(x => x.Id == 6);
            //var adelMusic1 = context.Musics.FirstOrDefault(y => y.Id == 388);
            //var adelMusic2 = context.Musics.FirstOrDefault(y => y.Id == 385);

            //context.UserBehaviors.Add(new Models.UserBehavior()
            //{
            //    UserId = 6,
            //    MusicId = 388,
            //    ListeningCount = 5,
            //    NameOfDay = "Friday",
            //    TimeOfDay = "Morning"
            //});
            //context.UserBehaviors.Add(new Models.UserBehavior()
            //{
            //    UserId = 6,
            //    MusicId = 385,
            //    ListeningCount = 9,
            //    NameOfDay = "Friday",
            //    TimeOfDay = "Forenoon"
            //});

            //var ozil = context.Users.FirstOrDefault(x => x.Id == 5);
            //var ozilMusic1 = context.Musics.FirstOrDefault(y => y.Id == 1538);
            //var ozilMusic2 = context.Musics.FirstOrDefault(y => y.Id == 1571);

            //context.UserBehaviors.Add(new Models.UserBehavior()
            //{
            //    UserId = 5,
            //    MusicId = 1538,
            //    ListeningCount = 5,
            //    NameOfDay = "Friday",
            //    TimeOfDay = "Morning"
            //});
            //context.UserBehaviors.Add(new Models.UserBehavior()
            //{
            //    UserId = 5,
            //    MusicId = 1571,
            //    ListeningCount = 9,
            //    NameOfDay = "Friday",
            //    TimeOfDay = "Forenoon"
            //});

            context.SaveChanges();
        }
    }
}
