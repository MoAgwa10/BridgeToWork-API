using System.ComponentModel.DataAnnotations;

namespace B2W.Models.Authentication
{
    public class tokenrequest
    {
        [Required]
        public string email { get; set; }
    }
}
