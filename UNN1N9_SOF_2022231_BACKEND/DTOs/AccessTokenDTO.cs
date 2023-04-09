using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace UNN1N9_SOF_2022231_BACKEND.DTOs
{
    public class AccessTokenDTO
    {
        public int UserId { get; set; }
        public string Authorizationcode { get; set; }
    }
}
