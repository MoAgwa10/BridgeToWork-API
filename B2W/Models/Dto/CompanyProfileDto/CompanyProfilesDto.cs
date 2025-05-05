using B2W.Models.Dto.CompanyProfileDto;

public class CompanyProfilesDto
{
   
    
        public int CompanyProfileId { get; set; }

        public string CompanyName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? FieldOfWork { get; set; }

        public string? WebsiteUrl { get; set; }

        public string? SocialMediaLinks { get; set; }

        public string? Location { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string ApplicationUserId { get; set; } = null!;

        // Navigation properties (اختياريين للعرض فقط، مش للإضافة أو التعديل)
        public List<AccessibilityFeatureDto>? AccessibilityFeatures { get; set; }

        public List<CompanyReviewDto>? Reviews { get; set; }

        public List<CompanyEmployeeDto>? Employees { get; set; }
   
} 