using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using SpotifyAPI.Web;
using System.Collections.Specialized;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UNN1N9_SOF_2022231_BACKEND.Data;
using UNN1N9_SOF_2022231_BACKEND.DTOs;
using UNN1N9_SOF_2022231_BACKEND.Extensions;
using UNN1N9_SOF_2022231_BACKEND.Interfaces;
using UNN1N9_SOF_2022231_BACKEND.Models;

namespace UNN1N9_SOF_2022231_BACKEND.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper, IPhotoService photoService)
        {
            this._context = context;
            this._tokenService = tokenService;
            this._mapper = mapper;
            this._photoService = photoService;
        }

        [HttpGet("spotify-auth")]
        public async Task<ActionResult> SpotifyAuth()
        {
            


            return Ok();
        }
        [HttpPost("getaccesstoken")]
        public async Task<ActionResult> GetAccessToken(AccessTokenDTO accessToken)
        {
            string authorizationCode = accessToken.token;
            string redirectUri = "http://localhost:4200/spotify-success";
            string clientId = "1ec4eab22f26449491c0d514d9b464ef";
            string clientSecret = "ede6e9fc0b024434a1e9f6302f7873a4";

            Console.WriteLine(authorizationCode);

            ;

            


            RestClient client = new RestClient("https://accounts.spotify.com");
            RestRequest request = new RestRequest("api/token", Method.Post);
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("code", authorizationCode);
            request.AddParameter("redirect_uri", redirectUri);
            request.AddParameter("client_id", clientId);
            request.AddParameter("client_secret", clientSecret);

            RestResponse response = client.Execute(request);
            string responseBody = response.Content;

            ;

            if (responseBody.ToLower().Contains("error"))
            {
                return BadRequest();
            }
            else
            {
                var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == accessToken.userid);
                var cut = JsonConvert.DeserializeObject<ResponseDTO>(responseBody);
                ;
                user.SpotifyAccessToken = cut.access_token;
                ;
                _context.SaveChanges();
                return Ok();
            }

        }

        private void RetrieveAccessToken(AccessTokenDTO accessToken)
        {
            var user = _context.Users.SingleOrDefault(x => x.Id == accessToken.userid);

            string authorizationCode = accessToken.token;
            string redirectUri = "http://localhost:4200/spotify-success";
            string clientId = "1ec4eab22f26449491c0d514d9b464ef";
            string clientSecret = "ede6e9fc0b024434a1e9f6302f7873a4";

            RestClient client = new RestClient("https://accounts.spotify.com");
            RestRequest request = new RestRequest("api/token", Method.Post);
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("code", authorizationCode);
            request.AddParameter("redirect_uri", redirectUri);
            request.AddParameter("client_id", clientId);
            request.AddParameter("client_secret", clientSecret);

            RestResponse response = client.Execute(request);
            string responseBody = response.Content;

            if (!responseBody.ToLower().Contains("error"))
            {
                var cut = JsonConvert.DeserializeObject<ResponseDTO>(responseBody);
                user.SpotifyAccessToken = cut.access_token;
                _context.SaveChanges();
            }
        }

        [HttpGet("userpic/{id}")]
        public async Task<ActionResult<string>> GetUserPic(int id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == id);
            ;
            return user.PhotoUrl;
        }


        [HttpPut("spotifypic")]
        public async Task<ActionResult<UserDto>> SpotifyPic(AccessTokenDTO accessToken)
        {
            //terjunk vissza userdto-val es akkor lehet frissul
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == accessToken.userid);
            ;
            RetrieveAccessToken(accessToken);
            ;

            if (user.SpotifyAccessToken != null)
            {
                var client = new RestClient("https://api.spotify.com");
                var request = new RestRequest("/v1/me", Method.Get);
                request.AddHeader("Authorization", $"Bearer {user.SpotifyAccessToken}");

                var response = client.Execute(request);
                  ;
                if (response.IsSuccessful)
                {
                    ;
                    var spotyuser = JsonConvert.DeserializeObject<SpotifyAccountDTO>(response.Content);
                    ;
                    user.PhotoUrl = _photoService.TransformImage(spotyuser.images[0].url);
                    ;
                    _context.SaveChanges();
                    return new UserDto
                    {
                        Id = user.Id,
                        Username = user.UserName,
                        Country = user.Country,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        YearOfBirth = user.YearOfBirth,
                        Gender = user.Gender,
                        PhotoUrl = user.PhotoUrl,
                        Token = _tokenService.CreateToken(user)
                    };
                }
                else
                {
                    return BadRequest("Something went wrong!");
                }
            }
            else
            {
                return BadRequest("You need to authorize yourself first!");
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UsernameExistsChecker(registerDto.UserName))
                return BadRequest("Username is already taken.");

            if (await EmailExistsChecker(registerDto.Email))
                return BadRequest("Email is already in use.");


            var user = _mapper.Map<AppUser>(registerDto);

            using var hmac = new HMACSHA512();

            user.UserName = registerDto.UserName.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;
            user.PhotoUrl = "";
            user.PublicId = "";

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                Country = user.Country,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                YearOfBirth = user.YearOfBirth,
                Gender = user.Gender,
                PhotoUrl = user.PhotoUrl,
                Token = _tokenService.CreateToken(user)
            };

        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username);
            ;
            if (user == null)
                return Unauthorized("Invalid username!");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                    return Unauthorized("Invalid password!");
            }

            return new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                Country = user.Country,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                YearOfBirth = user.YearOfBirth,
                Gender = user.Gender,
                PhotoUrl = user.PhotoUrl,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPut("updateuser")]
        public async Task<ActionResult> UpdateUser(UserUpdateDto userUpdateDto)
        {
            var username = User.GetUsername();
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == username);

            if (user == null)
                return NotFound();

            _mapper.Map(userUpdateDto, user);

            if (await _context.SaveChangesAsync() > 0)
                return NoContent();

            return BadRequest("Failed to update user");
        }

        [HttpPost("addphoto")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file) 
        {
            var username = User.GetUsername();
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == username);

            if (user == null)
                return NotFound();

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null)
                return BadRequest(result.Error.Message);

            user.PhotoUrl = result.SecureUrl.AbsoluteUri;
            user.PublicId = result.PublicId;

            if (await _context.SaveChangesAsync() > 0)
                return _mapper.Map<PhotoDto>(user);

            return BadRequest("An error occured while uploading your photo. Please try again.");
        }

        private async Task<bool> UsernameExistsChecker(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
        private async Task<bool> EmailExistsChecker(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email.ToLower());
        }

    }
}
