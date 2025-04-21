using B2W.Models.Jop;
using B2W.Models.User;
using B2W.Models.UserCertifications;
using B2W.Models.UserComment;
using B2W.Models.Userpost;
using B2W.Models.UserProfilePic;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace B2W.Models.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        public virtual ICollection<Post> Post { get; set; } = new List<Post>();
        public virtual ICollection<Comment> Comment { get; set; } = new List<Comment>();
        public virtual ICollection<Jop.Jop> JopSeekers { get; set; } = new List<Jop.Jop>();
        public virtual ICollection<JopApply> JopApplies { get; set; } = new List<JopApply>();
        public virtual ICollection<UserProfilePicture> UserProfilePictures { get; set; } = new List<UserProfilePicture>();
        [ForeignKey("UserProfileId")]
        public int? UserProfileId { get; set; }
        public  UserProfile UserProfile { get; set; }

        [ForeignKey("CompanyProfileId")]
        public int? CompanyProfileId { get; set; }
        public CompanyProfile.CompanyProfile CompanyProfile { get; set; }



    }
}
