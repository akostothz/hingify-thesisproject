using UNN1N9_SOF_2022231_BACKEND.Models;

namespace UNN1N9_SOF_2022231_BACKEND.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
