public class UserProfileCreateDto
{
    public int id { get; set; } 
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Gender { get; set; } = null!;
    public string JobTitle { get; set; } = null!;
    public string JobType { get; set; } = null!;
    public string WorkModel { get; set; } = null!;
    public string ExperienceLevel { get; set; } = null!;
    public string? DesiredJobTitle { get; set; }
    public string? DisabilityType { get; set; }
    public string FontSize { get; set; }
    public bool DarkMode { get; set; }
    public string ApplicationUserId { get; set; } = null!;
}
