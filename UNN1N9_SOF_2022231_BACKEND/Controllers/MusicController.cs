using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        public async Task<ActionResult<MusicDto>> FindMusic(string trackId)
        {
            var music = await _logic.FindMusic(trackId);
            var musicToReturn = _mapper.Map<MusicDto>(music);

            return Ok(musicToReturn);
        }

        [HttpGet("{expr}")]
        public async Task<ActionResult<IEnumerable<MusicDto>>> Search(string expr)
        {
            var musics = await _logic.Search(expr);
            var musicsToReturn = _mapper.Map<IEnumerable<MusicDto>>(musics);

            return Ok(musicsToReturn);
        }

        [HttpPost]
        public async Task<ActionResult> LikeSong(BehaviorDto behaviorDto)
        {
            _logic.AddLikedSong(behaviorDto.UserId, behaviorDto.MusicId);
    
            return NoContent();
        }

    }
}
