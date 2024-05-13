using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Reflection.Metadata;
using UNN1N9_SOF_2022231_BACKEND.Data;
using UNN1N9_SOF_2022231_BACKEND.DTOs;
using UNN1N9_SOF_2022231_BACKEND.Errors;
using UNN1N9_SOF_2022231_BACKEND.Logic;
using UNN1N9_SOF_2022231_BACKEND.Models;
using UNN1N9_SOF_2022231_BACKEND.Services;
using static SpotifyAPI.Web.SearchRequest;

namespace HingifyTests
{
    [TestFixture]
    public class MusicTests
    {
        MusicLogic? logic;

        IQueryable<Music>? musicData;
        Mock<DbSet<Music>>? musicMockSet;
        IQueryable<AppUser>? userData;
        Mock<DbSet<AppUser>>? userMockSet;
        IQueryable<LikedSong>? likedSongData;
        Mock<DbSet<LikedSong>>? likedSongMockSet;
        IQueryable<UserBehavior>? userBehaviourData;
        Mock<DbSet<UserBehavior>>? userBehaviourMockSet;

        Mock<IDataContext>? mockContext;

        [SetUp]
        public void Setup()
        {
           // Mocks for Music
           musicData = new List<Music>
           {
                new Music { Id = 1, Genre = "Rock", ArtistName = "Jimi Hendrix", TrackName = "Spanish Castle Magic", TrackId = "2KFE98Iw0X23sf4vJYcbLH", Acousticness = 0.0504, Danceability = 0.405, DurationMs = 243320, Energy = 0.805, Key = "F", Liveness = 0.805, Loudness = -5.293, Mode = "Minor", Speechiness = 0.0671, Tempo = 131, Valence = 0.805 },
                new Music { Id = 2, Genre = "Rock", ArtistName = "7eventh Time Down", TrackName = "The 99", TrackId = "11DJ8aTa16v6VYYuDdzJH6", Acousticness = 0.05, Danceability = 0.404, DurationMs = 201076, Energy = 0.809, Key = "C", Liveness = 0.800, Loudness = -8.465, Mode = "Minor", Speechiness = 0.0655, Tempo = 188, Valence = 0.800 },
                new Music { Id = 3, Genre = "Rock", ArtistName = "Nirvana", TrackName = "School - Remastered", TrackId = "0vVceF7KQkSfjn11enSdJy", Acousticness = 0.0515, Danceability = 0.208, DurationMs = 162147, Energy = 0.964, Key = "E", Liveness = 0.0701, Loudness = -6.398, Mode = "Major", Speechiness = 0.103, Tempo = 165, Valence = 0.0954 },
                new Music { Id = 4, Genre = "Rock", ArtistName = "A Perfect Circle", TrackName = "The Doomed", TrackId = "44OUZyiPnJc4pOZw4J6pid", Acousticness = 0.0477, Danceability = 0.367, DurationMs = 281781, Energy = 0.903, Key = "E", Liveness = 0.235, Loudness = -5.956, Mode = "Minor", Speechiness = 0.175, Tempo = 106.329, Valence = 0.239 },
                new Music { Id = 5, Genre = "Rock", ArtistName = "Metallica", TrackName = "The Unforgiven III", TrackId = "6guXhXMAHU4QYaEsobnS6v", Acousticness = 0.00203, Danceability = 0.298, DurationMs = 466587, Energy = 0.766, Key = "E", Liveness = 0.117, Loudness = -3.485, Mode = "Minor", Speechiness = 0.0347, Tempo = 121.555, Valence = 0.0923 },
                new Music { Id = 6, Genre = "Rock", ArtistName = "Bring Me The Horizon", TrackName = "Run", TrackId = "1wvs9V8xEJStRzx78lSNJK", Acousticness = 0.00301, Danceability = 0.481, DurationMs = 222760, Energy = 0.938, Key = "B", Liveness = 0.0859, Loudness = -4.759, Mode = "Minor", Speechiness = 0.0781, Tempo = 134.962, Valence = 0.357 },
                new Music { Id = 7, Genre = "Rock", ArtistName = "Metallica", TrackName = "Moth Into Flame", TrackId = "5sEcwMeC3QDnSOWlyQyQ3E", Acousticness = 3.28E-05, Danceability = 0.169, DurationMs = 350644, Energy = 0.978, Key = "E", Liveness = 0.0567, Loudness = -3.37, Mode = "Minor", Speechiness = 0.0657, Tempo = 179.035, Valence = 0.38 },
                new Music { Id = 8, Genre = "Alternative", ArtistName = "Electric Guest", TrackName = "American Daydream", TrackId = "2r0lAM25q5tJE1H4SesviY", Acousticness = 0.172, Danceability = 0.673, DurationMs = 168627, Energy = 0.485, Key = "F", Liveness = 0.101, Loudness = -10.025, Mode = "Minor", Speechiness = 0.0386, Tempo = 75.032, Valence = 0.407 },
                new Music { Id = 9, Genre = "Alternative", ArtistName = "Morphine", TrackName = "Buena", TrackId = "2SSBJhvMsujY94GK5JAtKs", Acousticness = 0.106, Danceability = 0.576, DurationMs = 199667, Energy = 0.494, Key = "A#", Liveness = 0.13, Loudness = -12.242, Mode = "Minor", Speechiness = 0.0605, Tempo = 96.478, Valence = 0.587 },
                new Music { Id = 10, Genre = "Alternative", ArtistName = "Zoé", TrackName = "Luna - Live", TrackId = "7b3k8I1fncAzbk9PHnLkbX", Acousticness = 0.123, Danceability = 0.223, DurationMs = 280400, Energy = 0.459, Key = "A", Liveness = 0.741, Loudness = -9.327, Mode = "Minor", Speechiness = 0.0317, Tempo = 180.06, Valence = 0.151 },
                new Music { Id = 11, Genre = "Alternative", ArtistName = "Korn", TrackName = "Narcissistic Cannibal (feat. Skrillex & Kill the Noise)", TrackId = "65XY6Cx0263J5BPnY8mPyE", Acousticness = 0.000456, Danceability = 0.519, DurationMs = 190707, Energy = 0.892, Key = "B", Liveness = 0.572, Loudness = -5.285, Mode = "Minor", Speechiness = 0.113, Tempo = 174.027, Valence = 0.559 },
                new Music { Id = 12, Genre = "Alternative", ArtistName = "A Perfect Circle", TrackName = "Gravity", TrackId = "1CO4BB8CaiQggtJ0R6GwGt", Acousticness = 0.018, Danceability = 0.607, DurationMs = 308067, Energy = 0.566, Key = "F#", Liveness = 0.101, Loudness = -8.526, Mode = "Major", Speechiness = 0.0288, Tempo = 123.945, Valence = 0.205 },
                new Music { Id = 13, Genre = "Alternative", ArtistName = "In This Moment", TrackName = "Black Wedding (feat. Rob Halford) - Edit", TrackId = "3ok7ZCCUKgjnB9HaqFde9Z", Acousticness = 0.621, Danceability = 0.678, DurationMs = 218174, Energy = 0.57, Key = "C#", Liveness = 0.09, Loudness = -3.225, Mode = "Minor", Speechiness = 0.13, Tempo = 136.979, Valence = 0.215 },
                new Music { Id = 14, Genre = "Alternative", ArtistName = "Joji", TrackName = "YEAH RIGHT", TrackId = "1VGzxJnVQND7Cg5H5wGj14", Acousticness = 0.619, Danceability = 0.672, DurationMs = 174358, Energy = 0.588, Key = "C#", Liveness = 0.0992, Loudness = -9.573, Mode = "Minor", Speechiness = 0.133, Tempo = 169, Valence = 0.204 },
                new Music { Id = 15, Genre = "Electronic", ArtistName = "Liquid Stranger", TrackName = "Burn Like Sun - Feat. Leah Culver", TrackId = "1aAFjtv448pOyhurajhQ1i", Acousticness = 0.16, Danceability = 0.613, DurationMs = 222707, Energy = 0.498, Key = "E", Liveness = 0.145, Loudness = -5.809, Mode = "Minor", Speechiness = 0.0279, Tempo = 139.052, Valence = 0.2 },
                new Music { Id = 16, Genre = "Electronic", ArtistName = "Little Dragon", TrackName = "Gravity", TrackId = "0IBQvbuotZ73h0Y4ObeGLL", Acousticness = 0.239, Danceability = 0.404, DurationMs = 458320, Energy = 0.517, Key = "F#", Liveness = 0.215, Loudness = -12.336, Mode = "Minor", Speechiness = 0.0471, Tempo = 162.879, Valence = 0.0335 },
                new Music { Id = 17, Genre = "Electronic", ArtistName = "Wax Tailor", TrackName = "Back on Wax", TrackId = "0i93KhAI7MZTrZAyU6AT5L", Acousticness = 0.491, Danceability = 0.835, DurationMs = 202523, Energy = 0.78, Key = "F", Liveness = 0.263, Loudness = -6.555, Mode = "Minor", Speechiness = 0.267, Tempo = 103.109, Valence = 0.789 },
                new Music { Id = 18, Genre = "Electronic", ArtistName = "Borgore", TrackName = "Best - Parker Remix", TrackId = "28xuMc4dLnVRKFqTSvuj4A", Acousticness = 0.0416, Danceability = 0.628, DurationMs = 153600, Energy = 0.948, Key = "B", Liveness = 0.162, Loudness = -2.584, Mode = "Minor", Speechiness = 0.0471, Tempo = 149.954, Valence = 0.322 },
                new Music { Id = 19, Genre = "Electronic", ArtistName = "NOËP", TrackName = "New Heights", TrackId = "3ICpGhrulCHC1rw0qvs27W", Acousticness = 0.255, Danceability = 0.684, DurationMs = 192209, Energy = 0.755, Key = "B", Liveness = 0.175, Loudness = -7.352, Mode = "Major", Speechiness = 0.267, Tempo = 148.039, Valence = 0.394 },
                new Music { Id = 20, Genre = "Electronic", ArtistName = "G Jones", TrackName = "Soundtrack to the Machine", TrackId = "2o6Ofau1XicXEcREBZnhZI", Acousticness = 0.254, Danceability = 0.681, DurationMs = 162680, Energy = 0.765, Key = "C#", Liveness = 0.166, Loudness = -5.398, Mode = "Major", Speechiness = 0.265, Tempo = 150, Valence = 0.392 },
                //new Music { Id = 21, Genre = "Electronic", ArtistName = "Test Artist", TrackName = "Please work", TrackId = "3ICpGhrulCHC1rw0qvs27R", Acousticness = 0.257, Danceability = 0.687, DurationMs = 192209, Energy = 0.758, Key = "B", Liveness = 0.172, Loudness = -7.352, Mode = "Major", Speechiness = 0.264, Tempo = 148.039, Valence = 0.388 },
                //new Music { Id = 22, Genre = "Electronic", ArtistName = "Test", TrackName = "pls pls", TrackId = "2o6Ofau1XicXEcREBZnh2", Acousticness = 0.251, Danceability = 0.686, DurationMs = 162680, Energy = 0.768, Key = "C#", Liveness = 0.168, Loudness = -5.398, Mode = "Major", Speechiness = 0.262, Tempo = 150, Valence = 0.39 }
           }.AsQueryable();


           musicMockSet = new Mock<DbSet<Music>>();

            musicMockSet.As<IDbAsyncEnumerable<Music>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<Music>(musicData.GetEnumerator()));
            musicMockSet.As<IQueryable<Music>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Music>(musicData.Provider));
            musicMockSet.As<IQueryable<Music>>()
                .Setup(m => m.Expression)
                .Returns(musicData.Expression);
            musicMockSet.As<IQueryable<Music>>()
                .Setup(m => m.ElementType)
                .Returns(musicData.ElementType);
            musicMockSet.As<IQueryable<Music>>()
                .Setup(m => m.GetEnumerator())
                .Returns(() => musicData.GetEnumerator());


            // Mocks for users

            //userek:
            // 1. Hungary - Male - 17-25
            // 2. Austria - Male - 39-59
            // 3. Hungary - Female - 17-25
            // 4. Austria - Female - 39-59
            userData = new List<AppUser>
            {
                new AppUser { Id = 1, Email = "akost@gmail.com", UserName = "akos.toth", FirstName="Akos", LastName="Toth", YearOfBirth=2000, Country="Hungary", Gender="Male", PhotoUrl="http://res.cloudinary.com/dt8loqugk/image/upload/c_fill,g_face,h_400,w_400/v1/user-pics/oi6rtrvbz8e824knyskz.jpg", SpotifyAccessToken="AQB6Skegjbp4TFAwUtRjJKfgo6JaCVjkzFk1T_QR6x2LK7b5rdukVo8oWzuM4zdhK78EsMXImAV8d0csInaduRYc9l5y-ARwnNPvoemfmpZNK9KTKiwg2t_nsbUWhM0YVx3Vq3mYPNj_KzfOAlWTU_dzHQ9CMDUUxOY-qWOkgaol97_9TBlO5DJaFGguhoUAjot8meVVkYe80IvvGu-C3D3Z7PXyufmHQ_eOOcmThgOw0pK9-DUMPvbSmIaCxehhkOsk9R0Z9eVYr6fGv9GKIctvwdH0iGtmSjxGr9TdCz1yJBcx73vNG_A", SpotifyId="uc10ezq3w7dsb7gi6p1ynezkv"},
                new AppUser { Id = 2, Email = "pistak@gmail.com", UserName = "pista.kiss", FirstName="Pista", LastName="Kiss", YearOfBirth=1970, Country="Austria", Gender="Male", PhotoUrl="http://res.cloudinary.com/dt8loqugk/image/upload/c_fill,g_face,h_400,w_400/v1/user-pics/oi6rtrvbz8e824knyskz.jpg", SpotifyAccessToken="AQB6Skegjbp4TFAwUtRjJKfgo6JaCVjkzFk1T_QR6x2LK7b5rdukVo8oWzuM4zdhK78EsMXImAV8d0csInaduRYc9l5y-ARwnNPvoemfmpZNK9KTKiwg2t_nsbUWhM0YVx3Vq3mYPNj_KzfOAlWTU_dzHQ9CMDUUxOY-qWOkgaol97_9TBlO5DJaFGguhoUAjot8meVVkYe80IvvGu-C3D3Z7PXyufmHQ_eOOcmThgOw0pK9-DUMPvbSmIaCxehhkOsk9R0Z9eVYr6fGv9GKIctvwdH0iGtmSjxGr9TdCz1yJBcx73vNG_A", SpotifyId="uc10ezq3w7dsb7gi6p1ynezkv"},
                new AppUser { Id = 3, Email = "evi@gmail.com", UserName = "evelin.maa", FirstName="Evelin", LastName="Moldovan", YearOfBirth=2002, Country="Hungary", Gender="Female", PhotoUrl="http://res.cloudinary.com/dt8loqugk/image/upload/c_fill,g_face,h_400,w_400/v1/user-pics/oi6rtrvbz8e824knyskz.jpg", SpotifyAccessToken="AQB6Skegjbp4TFAwUtRjJKfgo6JaCVjkzFk1T_QR6x2LK7b5rdukVo8oWzuM4zdhK78EsMXImAV8d0csInaduRYc9l5y-ARwnNPvoemfmpZNK9KTKiwg2t_nsbUWhM0YVx3Vq3mYPNj_KzfOAlWTU_dzHQ9CMDUUxOY-qWOkgaol97_9TBlO5DJaFGguhoUAjot8meVVkYe80IvvGu-C3D3Z7PXyufmHQ_eOOcmThgOw0pK9-DUMPvbSmIaCxehhkOsk9R0Z9eVYr6fGv9GKIctvwdH0iGtmSjxGr9TdCz1yJBcx73vNG_A", SpotifyId="uc10ezq3w7dsb7gi6p1ynezkv"},
                new AppUser { Id = 4, Email = "pannan@gmail.com", UserName = "panna.nagy", FirstName="Panna", LastName="Nagy", YearOfBirth=1971, Country="Austria", Gender="Female", PhotoUrl="http://res.cloudinary.com/dt8loqugk/image/upload/c_fill,g_face,h_400,w_400/v1/user-pics/oi6rtrvbz8e824knyskz.jpg", SpotifyAccessToken="AQB6Skegjbp4TFAwUtRjJKfgo6JaCVjkzFk1T_QR6x2LK7b5rdukVo8oWzuM4zdhK78EsMXImAV8d0csInaduRYc9l5y-ARwnNPvoemfmpZNK9KTKiwg2t_nsbUWhM0YVx3Vq3mYPNj_KzfOAlWTU_dzHQ9CMDUUxOY-qWOkgaol97_9TBlO5DJaFGguhoUAjot8meVVkYe80IvvGu-C3D3Z7PXyufmHQ_eOOcmThgOw0pK9-DUMPvbSmIaCxehhkOsk9R0Z9eVYr6fGv9GKIctvwdH0iGtmSjxGr9TdCz1yJBcx73vNG_A", SpotifyId="uc10ezq3w7dsb7gi6p1ynezkv"},
                new AppUser { Id = 5, Email = "cluster@gmail.com", UserName = "cluster", FirstName="Cluster", LastName="Test", YearOfBirth=2020, Country="United Kingdom", Gender="Other", PhotoUrl="http://res.cloudinary.com/dt8loqugk/image/upload/c_fill,g_face,h_400,w_400/v1/user-pics/oi6rtrvbz8e824knyskz.jpg", SpotifyAccessToken="AQB6Skegjbp4TFAwUtRjJKfgo6JaCVjkzFk1T_QR6x2LK7b5rdukVo8oWzuM4zdhK78EsMXImAV8d0csInaduRYc9l5y-ARwnNPvoemfmpZNK9KTKiwg2t_nsbUWhM0YVx3Vq3mYPNj_KzfOAlWTU_dzHQ9CMDUUxOY-qWOkgaol97_9TBlO5DJaFGguhoUAjot8meVVkYe80IvvGu-C3D3Z7PXyufmHQ_eOOcmThgOw0pK9-DUMPvbSmIaCxehhkOsk9R0Z9eVYr6fGv9GKIctvwdH0iGtmSjxGr9TdCz1yJBcx73vNG_A", SpotifyId="uc10ezq3w7dsb7gi6p1ynezkv"},
                new AppUser { Id = 6, Email = "cluster2@gmail.com", UserName = "clusterMixed", FirstName="Cluster", LastName="Mixed", YearOfBirth=2020, Country="United Kingdom", Gender="Other", PhotoUrl="http://res.cloudinary.com/dt8loqugk/image/upload/c_fill,g_face,h_400,w_400/v1/user-pics/oi6rtrvbz8e824knyskz.jpg", SpotifyAccessToken="AQB6Skegjbp4TFAwUtRjJKfgo6JaCVjkzFk1T_QR6x2LK7b5rdukVo8oWzuM4zdhK78EsMXImAV8d0csInaduRYc9l5y-ARwnNPvoemfmpZNK9KTKiwg2t_nsbUWhM0YVx3Vq3mYPNj_KzfOAlWTU_dzHQ9CMDUUxOY-qWOkgaol97_9TBlO5DJaFGguhoUAjot8meVVkYe80IvvGu-C3D3Z7PXyufmHQ_eOOcmThgOw0pK9-DUMPvbSmIaCxehhkOsk9R0Z9eVYr6fGv9GKIctvwdH0iGtmSjxGr9TdCz1yJBcx73vNG_A", SpotifyId="uc10ezq3w7dsb7gi6p1ynezkv"},
            }.AsQueryable();

            userMockSet = new Mock<DbSet<AppUser>>();
            userMockSet.As<IDbAsyncEnumerable<AppUser>>().Setup(m => m.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<AppUser>(userData.GetEnumerator()));
            userMockSet.As<IQueryable<AppUser>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<AppUser>(userData.Provider));
            userMockSet.As<IQueryable<AppUser>>().Setup(m => m.Expression).Returns(userData.Expression);
            userMockSet.As<IQueryable<AppUser>>().Setup(m => m.ElementType).Returns(userData.ElementType);
            userMockSet.As<IQueryable<AppUser>>().Setup(m => m.GetEnumerator()).Returns(() => userData.GetEnumerator());
            
            
            // Mocks for LikedSongs

            likedSongData = new List<LikedSong>
            {
                new LikedSong { UserId = 1, MusicId = 1 }
            }.AsQueryable();

            likedSongMockSet = new Mock<DbSet<LikedSong>>();
            likedSongMockSet.As<IDbAsyncEnumerable<LikedSong>>().Setup(m => m.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<LikedSong>(likedSongData.GetEnumerator()));
            likedSongMockSet.As<IQueryable<LikedSong>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<LikedSong>(likedSongData.Provider));
            likedSongMockSet.As<IQueryable<LikedSong>>().Setup(m => m.Expression).Returns(likedSongData.Expression);
            likedSongMockSet.As<IQueryable<LikedSong>>().Setup(m => m.ElementType).Returns(likedSongData.ElementType);
            likedSongMockSet.As<IQueryable<LikedSong>>().Setup(m => m.GetEnumerator()).Returns(() => likedSongData.GetEnumerator());


            // Mocks for UserBehaviours

            userBehaviourData = new List<UserBehavior>
            {
                new UserBehavior { Id = 1, UserId = 1, MusicId = 1, ListeningCount = 2, NameOfDay = DateTime.Now.DayOfWeek.ToString(), TimeOfDay = TimeOfDayConverter(), Date = DateOnly.FromDateTime(DateTime.UtcNow) },
                new UserBehavior { Id = 2, UserId = 1, MusicId = 8, ListeningCount = 1, NameOfDay = DateTime.Now.DayOfWeek.ToString(), TimeOfDay = TimeOfDayConverter(), Date = DateOnly.FromDateTime(DateTime.UtcNow) },
                new UserBehavior { Id = 3, UserId = 2, MusicId = 1, ListeningCount = 1, NameOfDay = DateTime.Now.DayOfWeek.ToString(), TimeOfDay = TimeOfDayConverter(), Date = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-7) },
                new UserBehavior { Id = 4, UserId = 2, MusicId = 8, ListeningCount = 1, NameOfDay = DateTime.Now.DayOfWeek.ToString(), TimeOfDay = TimeOfDayConverter(), Date = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-7) },
                new UserBehavior { Id = 5, UserId = 2, MusicId = 13, ListeningCount = 1, NameOfDay = DateTime.Now.DayOfWeek.ToString(), TimeOfDay = TimeOfDayConverter(), Date = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-7) },
                new UserBehavior { Id = 6, UserId = 3, MusicId = 10, ListeningCount = 1, NameOfDay = DateTime.Now.DayOfWeek.ToString(), TimeOfDay = TimeOfDayConverter(), Date = DateOnly.FromDateTime(DateTime.UtcNow) },
                new UserBehavior { Id = 7, UserId = 5, MusicId = 18, ListeningCount = 1, NameOfDay = DateTime.Now.DayOfWeek.ToString(), TimeOfDay = TimeOfDayConverter(), Date = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-7) },
                new UserBehavior { Id = 8, UserId = 5, MusicId = 19, ListeningCount = 1, NameOfDay = DateTime.Now.DayOfWeek.ToString(), TimeOfDay = TimeOfDayConverter(), Date = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-7) },
                new UserBehavior { Id = 9, UserId = 6, MusicId = 18, ListeningCount = 1, NameOfDay = DateTime.Now.DayOfWeek.ToString(), TimeOfDay = TimeOfDayConverter(), Date = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-7) },
                new UserBehavior { Id = 10, UserId = 6, MusicId = 1, ListeningCount = 1, NameOfDay = DateTime.Now.DayOfWeek.ToString(), TimeOfDay = TimeOfDayConverter(), Date = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-7) },
                new UserBehavior { Id = 11, UserId = 6, MusicId = 14, ListeningCount = 1, NameOfDay = DateTime.Now.DayOfWeek.ToString(), TimeOfDay = TimeOfDayConverter(), Date = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-7) },
            }.AsQueryable();

            userBehaviourMockSet = new Mock<DbSet<UserBehavior>>();
            userBehaviourMockSet.As<IDbAsyncEnumerable<UserBehavior>>().Setup(m => m.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<UserBehavior>(userBehaviourData.GetEnumerator()));
            userBehaviourMockSet.As<IQueryable<UserBehavior>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<UserBehavior>(userBehaviourData.Provider));
            userBehaviourMockSet.As<IQueryable<UserBehavior>>().Setup(m => m.Expression).Returns(userBehaviourData.Expression);
            userBehaviourMockSet.As<IQueryable<UserBehavior>>().Setup(m => m.ElementType).Returns(userBehaviourData.ElementType);
            userBehaviourMockSet.As<IQueryable<UserBehavior>>().Setup(m => m.GetEnumerator()).Returns(() => userBehaviourData.GetEnumerator());


            // mock kontextus beállítása
            mockContext = new Mock<IDataContext>();
            mockContext
                .Setup((m) => m.Musics)
                .Returns(musicMockSet?.Object);
            mockContext
                .Setup((u) => u.Users)
                .Returns(userMockSet?.Object);
            mockContext
                .Setup((l) => l.LikedSongs)
                .Returns(likedSongMockSet?.Object);
            mockContext
                .Setup((ub) => ub.UserBehaviors)
                .Returns(userBehaviourMockSet?.Object);

            var mockOptions = new Mock<IOptions<SpotifySettings>>();
            mockOptions.Setup(x => x.Value).Returns(new SpotifySettings());

            logic = new MusicLogic(mockContext.Object, mockOptions.Object);

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

        [TestCase]
        public async Task ClusterTest()
        {
            //az első 3 dal az amit hallgatott -> ezek lesznek legelől, majd a másik kettő pedig távolság alapján helyezkedik el
            var expectedFoundSongs = new List<Music>
            {
               new Music { ArtistName = "Borgore", TrackName = "Best - Parker Remix", TrackId = "28xuMc4dLnVRKFqTSvuj4A", DurationMs = 153600 },
               new Music { ArtistName = "Joji", TrackName = "YEAH RIGHT", TrackId = "1VGzxJnVQND7Cg5H5wGj14", DurationMs = 174358 },
               new Music { ArtistName = "Jimi Hendrix", TrackName = "Spanish Castle Magic", TrackId = "2KFE98Iw0X23sf4vJYcbLH", DurationMs = 243320 },
               new Music { ArtistName = "7eventh Time Down", TrackName = "The 99", TrackId = "11DJ8aTa16v6VYYuDdzJH6", DurationMs = 201076 },
               new Music { ArtistName = "In This Moment", TrackName = "Black Wedding (feat. Rob Halford) - Edit", TrackId = "3ok7ZCCUKgjnB9HaqFde9Z", DurationMs = 218174 },
            };

            var foundSongs = await logic.GetPersonalizedMixCluster(6);

            CollectionAssert.AreEquivalent(expectedFoundSongs, foundSongs);
        }

        [TestCase]
        public void IsLikedTest()
        {
            Assert.IsTrue(logic.IsLiked(1, 1));
        }

        [TestCase]
        public void AddLikedSongTest()
        {
            logic.AddLikedSong(1, 4);
            likedSongMockSet.Verify(m => m.Add(It.IsAny<LikedSong>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestCase]
        public void RemoveLikedSongTest()
        {
            logic.AddLikedSong(1, 6);
            likedSongMockSet.Verify(m => m.Add(It.IsAny<LikedSong>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());

            logic.RemoveFromLikedSong(1, 6);
            likedSongMockSet.Verify(m => m.Remove(It.IsAny<LikedSong>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Exactly(2));
        }

        [Test]
        public async Task GetStylesTest()
        {
            // 1. felhasználónak a stílusainak megszerzése
            var styles = await logic.GetStyles(1);

            // az 1. felhasználónak 2db hozzáadott stílusa van: Rock és Alternative
            Assert.AreEqual(new List<string> { "Rock", "Alternative" }, styles);
        }

        [TestCase]
        public async Task GetPersonalizedMixTest()
        {
            // az 2. felhasználónak 3 hallgatott zenéje van múlthétről: ezekhez 1-2db olyan zene van, ami elég közel van ahhoz, hogy kilistázza
            //  2 stílus van, így nem kevert megoldás lesz, hanem a legtöbbet hallgatott stílusból: itt így Alternative (2 zene az előző héten)
            // így csak Alternative-k lesznek a mixben, ebben benne kéne lennie az eredeti "American Daydream" - Electric Guest számnak és a másik hallgatott "Gravity" - A Perfect Circle -nek valamint "YEAH RIGHT" Joji-tól (ez az ami elég közel van hogy kiválassza)
            // mivel ezek vannak ugyan olyan stílusban és ezeknek van az euklidészi távolságuk a tartományban

            var expectedSongs = new List<Music>
            {
                new Music { Id = 200, Genre = "Alternative", ArtistName = "Electric Guest", TrackName = "American Daydream", TrackId = "2r0lAM25q5tJE1H4SesviY", DurationMs = 168627 },
                new Music { Id = 300, Genre = "Alternative", ArtistName = "In This Moment", TrackName = "Black Wedding (feat. Rob Halford) - Edit", TrackId = "3ok7ZCCUKgjnB9HaqFde9Z", DurationMs = 218174 },
                new Music { Id = 400, Genre = "Alternative", ArtistName = "Joji", TrackName = "YEAH RIGHT", TrackId = "1VGzxJnVQND7Cg5H5wGj14", DurationMs = 174358 },
            };

            var songs = await logic.GetPersonalizedMix(2);

           Assert.AreEqual(expectedSongs, songs);
        }

        [TestCase]
        public async Task FindMoreTest()
        {
            // NOËP "New Heights" zenéjéhez keresünk hasonlókat -> csak egyetlen ilyen lesz -> G Jones - "Soundtrack to the Machine"

            List<Music> expectedFoundSongs = new List<Music>
            {
               new Music { Id = 40, Genre = "Electronic", ArtistName = "NOËP", TrackName = "New Heights", TrackId = "3ICpGhrulCHC1rw0qvs27W", DurationMs = 192209 },
               new Music { Id = 50, Genre = "Electronic", ArtistName = "G Jones", TrackName = "Soundtrack to the Machine", TrackId = "2o6Ofau1XicXEcREBZnhZI", DurationMs = 162680, }
            };

            var songs = await logic.FindMore("3ICpGhrulCHC1rw0qvs27W");

            Assert.AreEqual(expectedFoundSongs, songs);
        }

        [TestCase]
        public async Task FindMusicTest()
        {
            // ez csak arra van, hogy pontosabb információkat jelenítsen meg az oldalon, ne csak amik kellenek

            Music expectedFoundSong = new Music
            {
                Id = 50,
                Genre = "Alternative",
                ArtistName = "Joji",
                TrackName = "YEAH RIGHT",
                TrackId = "1VGzxJnVQND7Cg5H5wGj14",
                DurationMs = 174358,
            };

            //generated output
            var foundSongs = await logic.FindMusic("1VGzxJnVQND7Cg5H5wGj14");

            Assert.AreEqual(expectedFoundSong, foundSongs.ToArray()[0]);
        }

        [TestCase]
        public async Task SearchTest()
        {
            // itt olyan zenékre keresünk, amiben benne vagy hogy 'feat' (előadó vagy cím)
            // a kontextusunkban ez a 3 ilyen
            var expectedFoundSongs = new List<Music>
            {
               new Music { ArtistName = "Liquid Stranger", TrackName = "Burn Like Sun - Feat. Leah Culver", TrackId = "1aAFjtv448pOyhurajhQ1i", DurationMs = 222707 },
               new Music { ArtistName = "In This Moment", TrackName = "Black Wedding (feat. Rob Halford) - Edit", TrackId = "3ok7ZCCUKgjnB9HaqFde9Z", DurationMs = 218174 },
               new Music { ArtistName = "Korn", TrackName = "Narcissistic Cannibal (feat. Skrillex & Kill the Noise)", TrackId = "65XY6Cx0263J5BPnY8mPyE", DurationMs = 190707 },
            };

            // ez pedig már az összes adatra lefuttatva visszaadott eredmény
            var foundSongs = await logic.Search("feat");

            // a sorrend nem számít, csak hogy mi van benne
            CollectionAssert.AreEquivalent(expectedFoundSongs, foundSongs);
        }

        [TestCase]
        public async Task GetLikedSongsTest()
        {
            // az 1. felhasználónak 1db kedvelt zenéje van:
            // Spanish Castle Magic by Jimi Hendrix

            //expected output
            List<Music> expectedLikedSongs = new List<Music>
            {
                new Music
                {
                    Id = 50,
                    Genre = "Rock",
                    ArtistName = "Jimi Hendrix",
                    TrackName = "Spanish Castle Magic",
                    TrackId = "2KFE98Iw0X23sf4vJYcbLH",
                    DurationMs = 183320,
                }
            };

            //generated output
            var likedSongs = await logic.GetActualLikedSongs(1);

            Assert.AreEqual(expectedLikedSongs, likedSongs);
        }

        [TestCase]
        public async Task GetMusicBySexTest()
        {
            //userek:
            // 1. Hungary - Male - 17-25
            // 2. Austria - Male - 39-59
            // 3. Hungary - Female - 17-25
            // 4. Austria - Female - 39-59

            // a 4. felhasználónak az 3. felhasználó zenéit kell megkapnia mivel mindketten Female neműek - azaz a hallgatott zenéit

            var expectedSongs = new List<Music>
            {
                new Music { Id = 100, Genre = "Alternative", ArtistName = "Zoé", TrackName = "Luna - Live", TrackId = "7b3k8I1fncAzbk9PHnLkbX", DurationMs = 280400 },
            };

            var songs = await logic.GetMusicsBySex(4);

            CollectionAssert.AreEquivalent(expectedSongs, songs);
        }

        [TestCase]
        public async Task GetMusicByCountryTest()
        {
            //userek:
            // 1. Hungary - Male - 17-25
            // 2. Austria - Male - 39-59
            // 3. Hungary - Female - 17-25
            // 4. Austria - Female - 39-59

            // a 3. felhasználónak az 1. felhasználó és a saját zenéit kell megkapnia mivel mind a ketten Hungary származásuak


            var expectedSongs = new List<Music>
            {
                new Music { Id = 10, Genre = "Rock", ArtistName = "Jimi Hendrix", TrackName = "Spanish Castle Magic", TrackId = "2KFE98Iw0X23sf4vJYcbLH", DurationMs = 183320 },
                new Music { Id = 20, Genre = "Alternative", ArtistName = "Electric Guest", TrackName = "American Daydream", TrackId = "2r0lAM25q5tJE1H4SesviY", DurationMs = 168627 },
                new Music { Id = 30, Genre = "Alternative", ArtistName = "Zoé", TrackName = "Luna - Live", TrackId = "7b3k8I1fncAzbk9PHnLkbX", DurationMs = 280400 },
            };

            var songs = await logic.GetMusicsByCountry(3);

            CollectionAssert.AreEquivalent(expectedSongs, songs);
        }

        [TestCase]
        public async Task GetMusicByAgeGroupTest()
        {
            //userek:
            // 1. Hungary - Male - 17-25
            // 2. Austria - Male - 39-59
            // 3. Hungary - Female - 17-25
            // 4. Austria - Female - 39-59

            // a 3. felhasználónak az 1. felhasználó és a saját zenéit kell megkapnia mivel mind a ketten a 17-25 korosztályban vannak

            var expectedSongs = new List<Music>
            {
                new Music { Id = 10, Genre = "Rock", ArtistName = "Jimi Hendrix", TrackName = "Spanish Castle Magic", TrackId = "2KFE98Iw0X23sf4vJYcbLH", DurationMs = 183320 },
                new Music { Id = 20, Genre = "Alternative", ArtistName = "Electric Guest", TrackName = "American Daydream", TrackId = "2r0lAM25q5tJE1H4SesviY", DurationMs = 168627 },
                new Music { Id = 30, Genre = "Alternative", ArtistName = "Zoé", TrackName = "Luna - Live", TrackId = "7b3k8I1fncAzbk9PHnLkbX", DurationMs = 280400 },
            };

            var songs = await logic.GetMusicsByAgeGroup(3);

            CollectionAssert.AreEquivalent(expectedSongs, songs);
        }

        [TestCase]
        public async Task GetDailyStatisticsTest()
        {
            // 1. ID-jű felhasználónak 2db behavior-ja van, melyeket 2x-1x hallgatott meg
            // ezek Rock és Alternative stílusúak: 4 és 3 percesek, a Rock hosszabb így az lesz az első
            //  Mivel csak 2 zene van, így a többi '-' karakterű lesz
            // típus Daily lesz

            var dailyStats = await logic.GetDailyStatistics2(1);

            StatDto expectedStatDto = new StatDto
            {
                Type = "Daily",
                MinsSpent = 11,
                NumOfListenedGenre = 2,
                MostListenedGenre = "Rock",
                MostListenedArtist = "Jimi Hendrix",
                MostListenedSong = "Spanish Castle Magic",
                SecondMostListenedGenre = "Alternative",
                SecondMostListenedArtist = "Electric Guest",
                SecondMostListenedSong = "American Daydream",
                ThirdMostListenedGenre = "-",
                ThirdMostListenedArtist = "-",
                ThirdMostListenedSong = "-",
                FourthMostListenedGenre = "-",
                FourthMostListenedArtist = "-",
                FourthMostListenedSong = "-",
                FifthMostListenedGenre = "-",
                FifthMostListenedArtist = "-",
                FifthMostListenedSong = "-",
            };

            Assert.AreEqual(expectedStatDto, dailyStats.ToArray()[0]);
        }

        [TestCase]
        public async Task GetWeeklyStatisticsTest()
        {
            //ugyan az elv, mint a napinál de ez itt 1 hetet néz:
            // az eredménynek ugyan annak kell lennie, leszámítva a típust

            var weeklyStats = await logic.GetWeeklyStatistics2(1);

            StatDto expectedStatDto = new StatDto
            {
                Type = "Weekly",
                MinsSpent = 11,
                NumOfListenedGenre = 2,
                MostListenedGenre = "Rock",
                MostListenedArtist = "Jimi Hendrix",
                MostListenedSong = "Spanish Castle Magic",
                SecondMostListenedGenre = "Alternative",
                SecondMostListenedArtist = "Electric Guest",
                SecondMostListenedSong = "American Daydream",
                ThirdMostListenedGenre = "-",
                ThirdMostListenedArtist = "-",
                ThirdMostListenedSong = "-",
                FourthMostListenedGenre = "-",
                FourthMostListenedArtist = "-",
                FourthMostListenedSong = "-",
                FifthMostListenedGenre = "-",
                FifthMostListenedArtist = "-",
                FifthMostListenedSong = "-",
            };

            Assert.AreEqual(expectedStatDto, weeklyStats.ToArray()[0]);
        }

        [TestCase]
        public async Task GetMonthlyStatisticsTest()
        {
            //ugyan az elv, mint a hetinél de ez itt 1 hónapot néz:
            // az eredménynek ugyan annak kell lennie, leszámítva a típust

            var monthlyStats = await logic.GetMonthlyStatistics2(1);

            StatDto expectedStatDto = new StatDto
            {
                Type = "Monthly",
                MinsSpent = 11,
                NumOfListenedGenre = 2,
                MostListenedGenre = "Rock",
                MostListenedArtist = "Jimi Hendrix",
                MostListenedSong = "Spanish Castle Magic",
                SecondMostListenedGenre = "Alternative",
                SecondMostListenedArtist = "Electric Guest",
                SecondMostListenedSong = "American Daydream",
                ThirdMostListenedGenre = "-",
                ThirdMostListenedArtist = "-",
                ThirdMostListenedSong = "-",
                FourthMostListenedGenre = "-",
                FourthMostListenedArtist = "-",
                FourthMostListenedSong = "-",
                FifthMostListenedGenre = "-",
                FifthMostListenedArtist = "-",
                FifthMostListenedSong = "-",
            };

            Assert.AreEqual(expectedStatDto, monthlyStats.ToArray()[0]);
        }

        [TestCase]
        public async Task GetYearlyStatisticsTest()
        {
            //ugyan az elv, mint a havinál de ez itt 1 évet néz:
            // az eredménynek ugyan annak kell lennie, leszámítva a típust

            var yearlyStats = await logic.GetYearlyStatistics2(1);

            StatDto expectedStatDto = new StatDto
            {
                Type = "Yearly",
                MinsSpent = 11,
                NumOfListenedGenre = 2,
                MostListenedGenre = "Rock",
                MostListenedArtist = "Jimi Hendrix",
                MostListenedSong = "Spanish Castle Magic",
                SecondMostListenedGenre = "Alternative",
                SecondMostListenedArtist = "Electric Guest",
                SecondMostListenedSong = "American Daydream",
                ThirdMostListenedGenre = "-",
                ThirdMostListenedArtist = "-",
                ThirdMostListenedSong = "-",
                FourthMostListenedGenre = "-",
                FourthMostListenedArtist = "-",
                FourthMostListenedSong = "-",
                FifthMostListenedGenre = "-",
                FifthMostListenedArtist = "-",
                FifthMostListenedSong = "-",
            };

            Assert.AreEqual(expectedStatDto, yearlyStats.ToArray()[0]);
        }

        [TestCase]
        public async Task GetLast7DaysTest()
        {
            // ez igazából a legutóbbi 7 nap dátumjait adja vissza String-ként

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            var expectedDates = new List<string>
            {
                today.AddDays(-6).ToString(),
                today.AddDays(-5).ToString(),
                today.AddDays(-4).ToString(),
                today.AddDays(-3).ToString(),
                today.AddDays(-2).ToString(),
                today.AddDays(-1).ToString(),
                today.ToString()
            };

            var dates = await logic.GetLast7Days(1);

            Assert.AreEqual(expectedDates, dates);
        }

        [TestCase]
        public async Task GetLast7DaysMinsTest()
        {
            // az 1. felhasználónak 2db hallgatott zenéje van: mind a kettő ma, egyik 2x, 11 perc összesen

            var expectedMinutes = new List<int> { 0, 0, 0, 0, 0, 0, 11 };

            var minutes = await logic.GetLast7DaysMins(1);

            Assert.AreEqual(expectedMinutes, minutes);
        }

        [TestCase]
        public async Task FindMoreByArtistTest()
        {
            //expected output
            // Joji dalait kellene viszakapnunk - azaz a Yeah Right-ot
            List<Music> expectedFoundSongs = new List<Music>
            {
                new Music
                {
                    Id = 50, 
                    Genre = "Alternative",
                    ArtistName = "Joji", 
                    TrackName = "YEAH RIGHT", 
                    TrackId = "1VGzxJnVQND7Cg5H5wGj14", 
                    DurationMs = 174358, 
                }
            };

            //generated output
            var foundSongs = await logic.FindMoreByArtist("Joji");

            Assert.AreEqual(expectedFoundSongs, foundSongs);
        }

        [TestCase]
        public async Task AddSongWithTrackIdTest()
        {
            // itt használja a Spotify-t, viszont ehhez scope-ok kellenének, így itt az Error-ra tesztelünk, aminek ebben az esetben dobódnia kellene
            SpotifyException ex = Assert.ThrowsAsync<SpotifyException>(async () => await logic.AddSong(1, "1CO4BB8CaiQggtJ0R6GwGt"));

            Assert.AreEqual(ex.Message, "Something went wrong while connecting to Spotify!");
        }

        [TestCase]
        public async Task AddSongWithListeningTest()
        {
            // itt használja a Spotify-t, viszont ehhez scope-ok kellenének, így itt az Error-ra tesztelünk, aminek ebben az esetben dobódnia kellene
            SpotifyException ex = Assert.ThrowsAsync<SpotifyException>(async () => await logic.AddSongWithListening(1));

            Assert.AreEqual(ex.Message, "Something went wrong while connecting to Spotify!");
        }

        [TestCase]
        public async Task AddBehaviourWithButtonTest()
        {
            AppUser user = await logic.GetUser(1);
            //itt cselesen van, itt token a zene id-ja
            AccessTokenDTO dto = new AccessTokenDTO { userid = 1, token = "1CO4BB8CaiQggtJ0R6GwGt" };
            
            logic.AddBehaviorWithButton(dto);
            userBehaviourMockSet.Verify(m => m.Add(It.IsAny<UserBehavior>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestCase]
        public async Task AddBehaviourWithListeningTest()
        {
            // itt használja a Spotify-t, viszont ehhez scope-ok kellenének, így itt az Error-ra tesztelünk, aminek ebben az esetben dobódnia kellene
            SpotifyException ex = Assert.ThrowsAsync<SpotifyException>(async () => await logic.AddBehaviorWithListening(1));

            Assert.AreEqual(ex.Message, "Something went wrong while connecting to Spotify!");
        }

        [TestCase]
        public async Task CreateSpotifyPlaylistTest()
        {
            AppUser user = await logic.GetUser(1);
            AccessTokenDTO dto = new AccessTokenDTO { userid = 1, token = user.SpotifyAccessToken };
            List<string> ids = new List<string> { "2KFE98Iw0X23sf4vJYcbLH", "2r0lAM25q5tJE1H4SesviY" };

            // itt használja a Spotify-t, viszont ehhez scope-ok kellenének, így itt az Error-ra tesztelünk, aminek ebben az esetben dobódnia kellene
            SpotifyException ex = Assert.ThrowsAsync<SpotifyException>(async () => await logic.CreateSpotifyPlaylist(dto, ids));
            
            Assert.AreEqual(ex.Message, "Something went wrong while connecting to Spotify!");
        }
    }
    internal class TestDbAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        internal TestDbAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new TestDbAsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestDbAsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(expression));
        }

        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
        {
            var expectedResultType = typeof(TResult).GetGenericArguments()[0];
            var executionResult = typeof(IQueryProvider)
                                 .GetMethod(
                                      name: nameof(IQueryProvider.Execute),
                                      genericParameterCount: 1,
                                      types: new[] { typeof(Expression) })
                                 .MakeGenericMethod(expectedResultType)
                                 .Invoke(this, new[] { expression });

            return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))
                                        ?.MakeGenericMethod(expectedResultType)
                                         .Invoke(null, new[] { executionResult });
        }

        //public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        //{
        //    return Task.FromResult(Execute<TResult>(expression));
        //}

    }

    internal class TestDbAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable<T>
    {
        public TestDbAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        public TestDbAsyncEnumerable(Expression expression)
            : base(expression)
        { }

        public IDbAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return new TestDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
        {
            return GetAsyncEnumerator();
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new TestDbAsyncQueryProvider<T>(this); }
        }
    }

    internal class TestDbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public TestDbAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public void Dispose()
        {
            _inner.Dispose();
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_inner.MoveNext());
        }

        public T Current
        {
            get { return _inner.Current; }
        }

        object IDbAsyncEnumerator.Current
        {
            get { return Current; }
        }
    }
}