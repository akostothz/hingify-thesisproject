using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using RestSharp;
using System.Security.AccessControl;
using UNN1N9_SOF_2022231_BACKEND.Data;
using UNN1N9_SOF_2022231_BACKEND.DTOs;
using UNN1N9_SOF_2022231_BACKEND.Interfaces;
using UNN1N9_SOF_2022231_BACKEND.Logic;
using UNN1N9_SOF_2022231_BACKEND.Models;

namespace UNN1N9_SOF_2022231_BACKEND.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("api/[controller]/[action]")]
    public class MusicController : ControllerBase
    {
        private readonly IMusicLogic _logic;
        private readonly IMapper _mapper;

        public MusicController(IMusicLogic logic, IMapper mapper)
        {
            _logic = logic;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> CreatePlaylist(List<string> mIds)
        {
            AccessTokenDTO accessToken = new AccessTokenDTO();
            ;
            for (int i = 0; i < mIds.Count(); i++)
            {
                if (i == 0)
                {
                    accessToken.userid = int.Parse(mIds[i]);
                    mIds.Remove(mIds[i]);
                }
                if (i == 1)
                {
                    accessToken.token = mIds[i];
                    mIds.Remove(mIds[i]);
                }
            }
            ;
            var user = await _logic.GetUser(accessToken.userid);
            _logic.RetrieveAccessToken(accessToken);
            ;
            HttpClient httpClient = new HttpClient();
            
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.SpotifyAccessToken);

            // Set up the request body
            var timeOfDay = _logic.TimeOfDayConverter();
            var day = DateTime.Now.DayOfWeek.ToString();
            DateOnly date = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var playlistName = $"{day}, {timeOfDay} by Hingify";
            var desc = $"This playlist was created for {user.UserName} by Hingify on {date}";
            var requestContent = new StringContent(JsonConvert.SerializeObject(new { name = playlistName, description = desc }));
            requestContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            // Send the POST request to create the playlist
            var response = await httpClient.PostAsync($"https://api.spotify.com/v1/users/{user.SpotifyId}/playlists", requestContent);
            response.EnsureSuccessStatusCode();

            // Get the response content
            var responseContent = await response.Content.ReadAsStringAsync();

            return Ok();
        }
       

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<MusicDto>>> GetPersonalizedMix(int id)
        {
            var musics = await _logic.GetPersonalizedMix(id);
            var musicsToReturn = _mapper.Map<IEnumerable<MusicDto>>(musics);

            return Ok(musicsToReturn);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<string>>> GetPlayedStyles(int id)
        {
            var styles = await _logic.GetStyles(id);

            return Ok(styles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<MusicDto>>> GetActualLikedSongs(int id)
        {
            var musics = await _logic.GetActualLikedSongs(id);
            var musicsToReturn = _mapper.Map<IEnumerable<MusicDto>>(musics);

            return Ok(musicsToReturn);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<MusicDto>>> GetLikedSongs(int id)
        {
            var musics = await _logic.GetLikedSongs(id);
            var musicsToReturn = _mapper.Map<IEnumerable<MusicDto>>(musics);

            return Ok(musicsToReturn);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<MusicDto>>> GetMusicsBySex(int id)
        {
            var musics = await _logic.GetMusicsBySex(id);
            var musicsToReturn = _mapper.Map<IEnumerable<MusicDto>>(musics);

            return Ok(musicsToReturn);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<MusicDto>>> GetMusicsByCountry(int id)
        {
            var musics = await _logic.GetMusicsByCountry(id);
            var musicsToReturn = _mapper.Map<IEnumerable<MusicDto>>(musics);

            return Ok(musicsToReturn);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<MusicDto>>> GetMusicsByAgeGroup(int id)
        {
            var musics = await _logic.GetMusicsByAgeGroup(id);
            var musicsToReturn = _mapper.Map<IEnumerable<MusicDto>>(musics);

            return Ok(musicsToReturn);
        }

        [HttpGet("{trackId}")]
        public async Task<ActionResult<IEnumerable<MusicDto>>> FindMore(string trackId)
        {
            var musics = await _logic.FindMore(trackId);
            var musicsToReturn = _mapper.Map<IEnumerable<MusicDto>>(musics);

            return Ok(musicsToReturn);
        }

        [HttpGet("{trackId}")]
        public async Task<ActionResult<IEnumerable<DetailedMusicDto>>> FindMusic(string trackId)
        {
            var music = await _logic.FindMusic(trackId);
            
            var musicToReturn = _mapper.Map<IEnumerable<DetailedMusicDto>>(music);

            return Ok(musicToReturn);
        }

        [HttpGet("{expr}")]
        public async Task<ActionResult<IEnumerable<MusicDto>>> FindMoreByArtist(string expr)
        {
            var musics = await _logic.FindMoreByArtist(expr);
            var musicsToReturn = _mapper.Map<IEnumerable<MusicDto>>(musics);

            return Ok(musicsToReturn);
        }

        [HttpGet("{expr}")]
        public async Task<ActionResult<IEnumerable<MusicDto>>> Search(string expr)
        {
            var musics = await _logic.Search(expr);
            var musicsToReturn = _mapper.Map<IEnumerable<MusicDto>>(musics);

            return Ok(musicsToReturn);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<MusicDto>>> GetDailyStatistics(int id)
        {
            var stats = await _logic.GetDailyStatistics(id);

            return Ok(stats);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<MusicDto>>> GetWeeklyStatistics(int id)
        {
            var stats = await _logic.GetWeeklyStatistics(id);

            return Ok(stats);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<MusicDto>>> GetMonthlyStatistics(int id)
        {
            var stats = await _logic.GetMonthlyStatistics(id);

            return Ok(stats);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<MusicDto>>> GetYearlyStatistics(int id)
        {
            var stats = await _logic.GetYearlyStatistics(id);

            return Ok(stats);
        }

        [HttpPost]
        public async Task<ActionResult> LikeSong(LikedSongDto likedSongDto)
        {
            _logic.AddLikedSong(likedSongDto.UserId, likedSongDto.MusicId);
    
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> DisikeSong(LikedSongDto likedSongDto)
        {
            _logic.RemoveFromLikedSong(likedSongDto.UserId, likedSongDto.MusicId);

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<bool>> IsLiked(LikedSongDto likedSongDto)
        {
            ;
            _logic.IsLiked(likedSongDto.UserId, likedSongDto.MusicId);

            return Ok(_logic.IsLiked(likedSongDto.UserId, likedSongDto.MusicId));
        }
    }
}
