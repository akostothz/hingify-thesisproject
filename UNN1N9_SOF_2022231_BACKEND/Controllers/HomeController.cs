using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UNN1N9_SOF_2022231_BACKEND.Data;
using UNN1N9_SOF_2022231_BACKEND.Models;

namespace UNN1N9_SOF_2022231_BACKEND.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly DataContext _context;

        public HomeController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        //[AllowAnonymous]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Music>>> GetMusics()
        {
            return await _context.Musics.ToListAsync();
        }

        //[HttpGet("{id}")]
        //[Authorize]
        //public async Task<ActionResult<IEnumerable<Music>>> GetMusic(string name)
        //{
        //    return await _context.Musics.FindAsync(name);
        //}


        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        //{
        //    return await _context.Users.ToListAsync();
        //}

        //// api/users/3
        //[HttpGet("{id}")]
        //public async Task<ActionResult<AppUser>> GetUser(int id)
        //{
        //    return await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
        //}
    }
}
