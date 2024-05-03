using Microsoft.AspNetCore.Identity;

namespace AskMentor.Helper
{
    public class Helper
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext context;
        public Helper(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            this.context = context;
        }

        public async Task<List<ApplicationUser>> GetUserByRole(string role)
        {
            return (List<ApplicationUser>)await _userManager.GetUsersInRoleAsync(role);
        }

        public async Task<string> GetUserId(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            return user.Id;
        }

        public async Task<string> GetNameUserByEmail(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            return user.Name;
        }

        public async Task<string> GetNameUserById(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            return user.Name;
        }


    }
}
