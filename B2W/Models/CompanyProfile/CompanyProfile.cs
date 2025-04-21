using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using B2W.Models.Authentication;

namespace B2W.Models.CompanyProfile
{

    public class CompanyProfile
    {
        public int CompanyProfileId { get; set; }

       

        public string CompanyName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? FieldOfWork { get; set; }

        public string? WebsiteUrl { get; set; }

        // تخزين الروابط كسلسلة مفصولة مثلاً بـ ;
        public string? SocialMediaLinks { get; set; }

        public string? Location { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [ForeignKey("ApplicationUserId")]
        public string ApplicationUserId { get; set; } 
        public virtual ApplicationUser ApplicationUser { get; set; } 

        // Navigation properties
        public List<AccessibilityFeature> AccessibilityFeatures { get; set; }
        public List<CompanyReview> Reviews { get; set; }
        public List<Jop.Jop> OpenedJobs { get; set; } = new();

        public List<CompanyEmployee> Employees { get; set; }
    }
}