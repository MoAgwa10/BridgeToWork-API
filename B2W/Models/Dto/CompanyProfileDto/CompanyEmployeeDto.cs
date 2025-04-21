public class CompanyEmployeeDto
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;
    public string JobType { get; set; } = string.Empty;
    public int UserProfileId { get; set; }
    public int CompanyProfileId { get; set; }
}