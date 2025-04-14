using B2W.Models.Authentication;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
        public string JobTitle { get; set; } // مسمى الوظيفة الحالي


        public string ProfileImageUrl { get; set; }

        public string JobType { get; set; } // Full-Time, Part-Time, Freelance, Contract
        public string WorkModel { get; set; } // On-site, Remote, Hybrid
        public string ExperienceLevel { get; set; } // Intern, Junior, Mid, Senior, Lead
        public string DesiredJobTitle { get; set; } // الوظيفة التي يسعى المستخدم إليها

        
        public string DisabilityType { get; set; } // Physical, Sensory, Learning, Developmental

     
        public string FontSize { get; set; } // Large, Medium, Small
        public bool DarkMode { get; set; } // هل الوضع الليلي مفعل أم لا


        [ForeignKey("ApplicationUserId")]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

    }
}
