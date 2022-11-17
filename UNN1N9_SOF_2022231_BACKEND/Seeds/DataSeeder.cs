using UNN1N9_SOF_2022231_BACKEND.Data;

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
    }
}
