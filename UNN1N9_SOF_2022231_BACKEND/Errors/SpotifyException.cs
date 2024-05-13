namespace UNN1N9_SOF_2022231_BACKEND.Errors
{
    public class SpotifyException : Exception
    {
        public SpotifyException() : base($"Something went wrong while connecting to Spotify!") { }
    }
}
