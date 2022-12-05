using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UNN1N9_SOF_2022231_BACKEND.Data;
using UNN1N9_SOF_2022231_BACKEND.Interfaces;
using UNN1N9_SOF_2022231_BACKEND.Models;

namespace UNN1N9_SOF_2022231_BACKEND.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MusicController : ControllerBase
    {
        private readonly DataContext _context;
        public MusicController(DataContext context)
        {
            this._context = context;
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Music>>> GetMusicsByAge(AppUser user)
        //{
        //    //átszervezni Logic-ba majd           
            
        //}
    }
}
