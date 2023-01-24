using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UNN1N9_SOF_2022231_BACKEND.DTOs;
using UNN1N9_SOF_2022231_BACKEND.Logic;

namespace UNN1N9_SOF_2022231_BACKEND.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserLogic _logic;
        private readonly IMapper _mapper;

        public UserController(IUserLogic logic, IMapper mapper)
        {
            _logic = logic;
            _mapper = mapper;
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(UserUpdateDto userUpdateDto)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _logic.GetUserByUsernameAsync(username);

            if (user == null)
                return NotFound();

            _mapper.Map(userUpdateDto, user);

            if (await _logic.SaveAllAsync())
                return NoContent();

            return BadRequest("Failed to update user");
        }
    }
}
