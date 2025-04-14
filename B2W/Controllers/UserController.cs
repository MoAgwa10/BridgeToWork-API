using B2W.UserService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace B2W.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserRepo _userRepo;

        public UserController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

      
        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            var users = _userRepo.GetAllUsers();
            return Ok(users);
        }

      
        [HttpGet("GetUser/{id}")]
        public IActionResult GetUserById(string id)
        {
            var user = _userRepo.GetUserById(id);
            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }





    }
}
