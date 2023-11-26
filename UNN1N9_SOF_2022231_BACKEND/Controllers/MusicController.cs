using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using RestSharp;
using System.Net.Http.Headers;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<BehaviorDto>>> AddBehaviorWithListening(int id)
        {
            var behavs = new List<UserBehavior>();
            var b = await _logic.AddBehaviorWithListening(id);
            behavs.Add(b);
            var behavsToReturn = _mapper.Map<IEnumerable<BehaviorDto>>(behavs);

            if (behavsToReturn.Count() == 0)
                return BadRequest();
            else
                return Ok(behavsToReturn);

        }

        [HttpGet("{ids}")]
        public async Task<ActionResult<IEnumerable<BehaviorDto>>> AddBehaviorWithButton(string ids) //azért kap ezt át, mert ebbe benne van az id, és a trackId-t is bele lehet rakni
        {
            string[] lines = ids.Split('.');
            AccessTokenDTO dto = new AccessTokenDTO() { userid = int.Parse(lines[0]), token = lines[1] };
            var behavs = new List<UserBehavior>();
            var b = await _logic.AddBehaviorWithButton(dto);
            behavs.Add(b);
            var behavsToReturn = _mapper.Map<IEnumerable<BehaviorDto>>(behavs);
            
            return Ok(behavsToReturn);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<MusicDto>>> AddSongWithListening(int id)
        {
            var musics = await _logic.AddSongWithListening(id);
            var musicsToReturn = _mapper.Map<IEnumerable<MusicDto>>(musics);

            return Ok(musicsToReturn);
        }

        [HttpGet("{cid}")]
        public async Task<ActionResult<IEnumerable<MusicDto>>> AddSongWithCid(string cid)
        {
            string[] line = cid.Split(';');
            int id = int.Parse(line[0]);
            string trackId = line[1];
            
            var musics = await _logic.AddSong(id, trackId);
            var musicsToReturn = _mapper.Map<IEnumerable<MusicDto>>(musics);

            return Ok(musicsToReturn);
        }

        [HttpPost]
        public async Task<ActionResult> CreatePlaylist(List<string> mIds)
        {
            AccessTokenDTO accessToken = new AccessTokenDTO();   

            for (int i = 0; i < mIds.Count(); i++)
            {
                if (i == 0)
                {
                    accessToken.userid = int.Parse(mIds[i]);
                    mIds.Remove(mIds[i]);
                }
                if (i == 1)
                {
                    accessToken.token = mIds[i - 1];
                    mIds.Remove(mIds[i - 1]);
                }
            }
            var playlist = await _logic.CreateSpotifyPlaylist(accessToken, mIds);

            if (playlist.Id == null)
                return BadRequest();
            else
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

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<String>>> GetLast7Days(int id)
        {
            ;
            var stats = await _logic.GetLast7Days(id);
            ;

            return Ok(stats);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<int>>> GetLast7DaysMins(int id)
        {
            ;
            var stats = await _logic.GetLast7DaysMins(id);
            ;

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
            _logic.IsLiked(likedSongDto.UserId, likedSongDto.MusicId);

            return Ok(_logic.IsLiked(likedSongDto.UserId, likedSongDto.MusicId));
        }
    }
}
