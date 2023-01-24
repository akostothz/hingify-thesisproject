using UNN1N9_SOF_2022231_BACKEND.Models;

namespace UNN1N9_SOF_2022231_BACKEND.Logic
{
    public interface IUserLogic
    {
        Task<AppUser> GetUserByUsernameAsync(string uname);
        Task<bool> SaveAllAsync();
        void Update(AppUser user);
    }
}