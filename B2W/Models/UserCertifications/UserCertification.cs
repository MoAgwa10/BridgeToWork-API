using B2W.Models.Authentication;
using B2W.Models.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace B2W.Models.UserCertifications
{
    public class UserCertification
    {
        [Key]
        public int CertificationId { get; set; }

        public string? Description { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public string? Image { get; set; }

        // ✅ الربط الجديد مع UserProfile
        [ForeignKey("UserProfile")]
        public int UserProfileId { get; set; }

        public UserProfile UserProfile { get; set; }

    }
}
