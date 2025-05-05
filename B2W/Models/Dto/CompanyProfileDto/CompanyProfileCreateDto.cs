public class CompanyProfileCreateDto
{
    public int CompanyProfileId { get; set; }
    public string CompanyName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FieldOfWork { get; set; } = null!;
    public string WebsiteUrl { get; set; } = null!;
    public string SocialMediaLinks { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
    public string ApplicationUserId { get; set; } = null!;
}