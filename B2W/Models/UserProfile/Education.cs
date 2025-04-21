using System.ComponentModel.DataAnnotations.Schema;
using B2W.Models.Authentication;
using Microsoft.Build.Framework;

namespace B2W.Models.User
{
    public class Education
    {

        public int Id { get; set; }

        [Required]
        public string University { get; set; }

        public string Faculty { get; set; }

        public string Degree { get; set; }

        [Required]
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

       
        [ForeignKey("UserProfileId")]
        public int UserProfileId { get; set; }

        public UserProfile UserProfile { get; set; }
    }
}
