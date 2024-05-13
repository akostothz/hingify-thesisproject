namespace UNN1N9_SOF_2022231_BACKEND.DTOs
{
    public class StatDto
    {
        public string Type { get; set; }
        public int MinsSpent { get; set; }
        public int NumOfListenedGenre { get; set; }
        public string MostListenedGenre { get; set; }
        public string MostListenedArtist { get; set; }
        public string MostListenedSong { get; set; }
        public string SecondMostListenedGenre { get; set; }
        public string SecondMostListenedArtist { get; set; }
        public string SecondMostListenedSong { get; set; }
        public string ThirdMostListenedGenre { get; set; }
        public string ThirdMostListenedArtist { get; set; }
        public string ThirdMostListenedSong { get; set; }
        public string FourthMostListenedGenre { get; set; }
        public string FourthMostListenedArtist { get; set; }
        public string FourthMostListenedSong { get; set; }
        public string FifthMostListenedGenre { get; set; }
        public string FifthMostListenedArtist { get; set; }
        public string FifthMostListenedSong { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            StatDto other = (StatDto)obj;

            return Type == other.Type &&
                   MinsSpent == other.MinsSpent &&
                   NumOfListenedGenre == other.NumOfListenedGenre &&
                   MostListenedGenre == other.MostListenedGenre &&
                   MostListenedArtist == other.MostListenedArtist &&
                   MostListenedSong == other.MostListenedSong &&
                   SecondMostListenedGenre == other.SecondMostListenedGenre &&
                   SecondMostListenedArtist == other.SecondMostListenedArtist &&
                   SecondMostListenedSong == other.SecondMostListenedSong &&
                   ThirdMostListenedGenre == other.ThirdMostListenedGenre &&
                   ThirdMostListenedArtist == other.ThirdMostListenedArtist &&
                   ThirdMostListenedSong == other.ThirdMostListenedSong &&
                   FourthMostListenedGenre == other.FourthMostListenedGenre &&
                   FourthMostListenedArtist == other.FourthMostListenedArtist &&
                   FourthMostListenedSong == other.FourthMostListenedSong &&
                   FifthMostListenedGenre == other.FifthMostListenedGenre &&
                   FifthMostListenedArtist == other.FifthMostListenedArtist &&
                   FifthMostListenedSong == other.FifthMostListenedSong;
        }
    }
}
