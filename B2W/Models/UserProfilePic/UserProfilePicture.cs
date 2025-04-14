using B2W.Models.Authentication;
using System.ComponentModel.DataAnnotations.Schema;

namespace B2W.Models.UserProfilePic
{
    public class UserProfilePicture
    {
        public int UserProfilePictureId { get; set; }
        public string UserId { get; set; }

        public byte[] Image { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;
    }
}
