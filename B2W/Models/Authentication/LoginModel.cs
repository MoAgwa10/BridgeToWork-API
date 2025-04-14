using System.ComponentModel.DataAnnotations;

namespace B2W.Models.Authentication
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
