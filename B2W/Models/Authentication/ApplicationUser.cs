using B2W.Models.Jop;
using B2W.Models.User;
using B2W.Models.UserCertifications;
using B2W.Models.UserComment;
using B2W.Models.Userpost;
using B2W.Models.UserProfilePic;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

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
        public UserProfile UserProfile { get; set; }
        public ICollection<Experience> Experience { get; set; }
        public ICollection<Education> Educations { get; set; }
        public ICollection<Skills> Skills { get; set; }
        public virtual ICollection<UserProfilePicture> UserProfilePictures { get; set; } = new List<UserProfilePicture>();
        public virtual ICollection<UserCertification> UserCertifications { get; set; } = new List<UserCertification>();



    }
}
