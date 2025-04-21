using B2W.Models.Authentication;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using B2W.Models.UserCertifications;
using B2W.Models.UserProfilePic;

namespace B2W.Models.User
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string Gender { get; set; }
        public  string JobTitle { get; set; } // المسمى الوظيفي الحالي
        public string JobType { get; set; } // Full-Time, Part-Time, Freelance, Contract
        public string WorkModel { get; set; } // On-site, Remote, Hybrid
        public string ExperienceLevel { get; set; } // Intern, Junior, Mid, Senior, Lead
        public string DesiredJobTitle { get; set; } // الوظيفة المستهدفة
        public string DisabilityType { get; set; } // نوع الإعاقة
        public string FontSize { get; set; } // Large, Medium, Small
        public bool DarkMode { get; set; } // تفعيل الوضع الليلي

        // ربط مع الـ ApplicationUser
        [ForeignKey("ApplicationUserId")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("UserCvId")]
        public int UserCvId { get; set; }

        public Cv UserCv { get; set; }

        // علاقات فرعية
        public ICollection<Experience> Experiences { get; set; } = new List<Experience>();
        public ICollection<Education> Educations { get; set; } = new List<Education>();
        public ICollection<Skills> Skills { get; set; } = new List<Skills>();
        public ICollection<Projects> Projects { get; set; }
        public ICollection <MillStones> MillStones { get; set; }
        public ICollection<UserProfilePicture> UserProfilePictures { get; set; } = new List<UserProfilePicture>();
        public ICollection<UserCertification> UserCertifications { get; set; } = new List<UserCertification>();
    }
}