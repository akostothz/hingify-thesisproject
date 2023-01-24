using Microsoft.EntityFrameworkCore;
using UNN1N9_SOF_2022231_BACKEND.Data;
using UNN1N9_SOF_2022231_BACKEND.Models;

namespace UNN1N9_SOF_2022231_BACKEND.Logic
{
    public class UserLogic : IUserLogic
    {
        DataContext _context;

        public UserLogic(DataContext context)
        {
            _context = context;
        }

        public async Task<AppUser> GetUserByUsernameAsync(string uname)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.UserName == uname);
        }
        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}
