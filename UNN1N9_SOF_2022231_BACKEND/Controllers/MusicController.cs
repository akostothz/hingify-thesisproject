using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.AccessControl;
using UNN1N9_SOF_2022231_BACKEND.Data;
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
        IMusicLogic _logic;

        public MusicController(IMusicLogic logic)
        {
            _logic = logic;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Music>>> GetPersonalizedMix(int id)
        {
            return Ok(await _logic.GetPersonalizedMix(id));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Music>>> GetLikedSongs(int id)
        {
            return Ok(await _logic.GetLikedSongs(id));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Music>>> GetMusicsBySex(int id)
        {
            return Ok(await _logic.GetMusicsBySex(id));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Music>>> GetMusicsByCountry(int id)
        {
            return Ok(await _logic.GetMusicsByCountry(id));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Music>>> GetMusicsByAgeGroup(int id)
        {
            return Ok(await _logic.GetMusicsByAgeGroup(id));
        }
        
    }
}
