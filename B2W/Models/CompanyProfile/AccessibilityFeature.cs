using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace B2W.Models.CompanyProfile
{
    public class AccessibilityFeature
    {
        [Key]
        public int Id { get; set; }

        public string FeatureName { get; set; }

        [ForeignKey("CompanyProfileId")]
        public int CompanyProfileId { get; set; }

        public CompanyProfile CompanyProfile { get; set; }
    }
}
