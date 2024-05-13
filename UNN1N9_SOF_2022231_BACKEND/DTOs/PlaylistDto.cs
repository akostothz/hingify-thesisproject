namespace UNN1N9_SOF_2022231_BACKEND.DTOs
{
    public class PlaylistDto
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            PlaylistDto other = (PlaylistDto)obj;

            return Name == other.Name;
        }
    }
}
