using System.ComponentModel.DataAnnotations.Schema;

namespace B2W.Models.Dto
{
    public class UserProfilePictureEditDto
    {
        

        [NotMapped]
        public IFormFile? Image { get; set; }  // New property for image upload
    }
}
