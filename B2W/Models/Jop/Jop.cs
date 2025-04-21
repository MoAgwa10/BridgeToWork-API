using B2W.Models.Authentication;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using B2W.Models.CompanyProfile;

namespace B2W.Models.Jop
{
    public class Jop
    {
        [Key]
        public int JopId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Requirments { get; set; } = string.Empty;

        [Required]
        public string AboutCompany { get; set; } = string.Empty;

        [Required]
        public string JopType { get; set; } = string.Empty;

        [Required]
        public string Level { get; set; } = string.Empty;

        [Required]
        public string WorkingModel { get; set; } = string.Empty;

        [Precision(18, 2)]
        public decimal? Salary { get; set; }

        // المستخدم الذي أنشأ الوظيفة
        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        [JsonIgnore]
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        // الطلبات المقدمة لهذه الوظيفة
        public virtual ICollection<JopApply> JopApplies { get; set; } = new List<JopApply>();

        [ForeignKey("CompanyProfileId")]
        public int CompanyProfileId { get; set; }

        public CompanyProfile.CompanyProfile CompanyProfile { get; set; }



    }
}
