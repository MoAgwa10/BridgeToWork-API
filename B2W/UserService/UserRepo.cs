using B2W.Models;
using B2W.Models.Authentication;
using Microsoft.AspNetCore.Identity;

namespace B2W.UserService
{
    public class UserRepo : IUserRepo
    {



        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager; 

        public UserRepo(ApplicationDbContext appdb, UserManager<ApplicationUser> userManager)
        {
            _db = appdb;
            _userManager = userManager;
        }

       
        public List<object> GetAllUsers()
        {
            return _db.Users
                .Select(user => new
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email
                }).ToList<object>();
        }

        
        public ApplicationUser GetUserById(string id)
        {
            return _db.Users.FirstOrDefault(u => u.Id == id);


        }
        public async Task<List<string>> GetUserIdsOnly()
        {
            var users = await _userManager.GetUsersInRoleAsync("User");//هنا انا بجددله الرول
            return users.Select(u => u.Id).ToList();


        }
    }
}
