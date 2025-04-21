using System.ComponentModel.DataAnnotations.Schema;
using B2W.Models.Authentication;
using Microsoft.Build.Framework;

namespace B2W.Models.User
{
    public class Experience
    {
        public int Id { get; set; }

        [Required]
        public string JobTitle { get; set; }

        [Required]
        public string OrganizationName { get; set; }

        [Required]
        public DateTime?StartDate { get; set; }

        public DateTime? EndDate { get; set; } 

        
        [ForeignKey("UserProfileId")]
        public int UserProfileId { get; set; }

        public UserProfile UserProfile { get; set; }
    }
}

