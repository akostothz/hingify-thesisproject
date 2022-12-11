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
    [Route("api/[controller]/[action]")]
    public class MusicController : ControllerBase
    {
        IMusicLogic _logic;

        public MusicController(IMusicLogic logic)
        {
            _logic = logic;
        }

        [HttpGet]
        public IEnumerable<Music> GetPersonalizedMix(int id)
        {
            return _logic.GetPersonalizedMix(id);
        }

        [HttpGet]
        public IEnumerable<Music> GetLikedSongs(int id)
        {
            return _logic.GetLikedSongs(id);
        }
        
        [HttpGet]
        public IEnumerable<Music> GetMusicsBySex(int id)
        {
            return _logic.GetMusicsBySex(id);
        }    

        [HttpGet]
        public IEnumerable<Music> GetMusicsByCountry(int id)
        {
            return _logic.GetMusicsByCountry(id);
        }

        [HttpGet]
        public IEnumerable<Music> GetMusicsByAgeGroup(int id)
        {
            return _logic.GetMusicsByAgeGroup(id);
        }
        
    }
}
