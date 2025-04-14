using B2W.Models.Authentication;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace B2W.Models.UserCertifications
{
    public class UserCertification
    {
        [Key]
        public int CertificationId { get; set; }

        public string UserId { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public string? Image { get; set; }


        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

    }
}
