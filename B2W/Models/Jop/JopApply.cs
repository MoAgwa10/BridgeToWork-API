using B2W.Models.Authentication;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace B2W.Models.Jop
{
    public class JopApply
    {
        [Key]
        public int JopApplyId { get; set; }

        [Required]
        [StringLength(200)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string CvFile { get; set; } = string.Empty;
        public string? AnyComment { get; set; } 


        // المستخدم الذي تقدم للوظيفة
        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        // الوظيفة التي تم التقديم عليها
        [Required]
        public int JopId { get; set; }

        [ForeignKey("JopId")]
        public virtual Jop JopSeeker { get; set; } = null!;

    }
}
