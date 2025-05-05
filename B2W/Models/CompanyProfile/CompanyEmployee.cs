using System.ComponentModel.DataAnnotations.Schema;
using B2W.Models.CompanyProfile;
using B2W.Models.User;
using Microsoft.Identity.Client;

public class CompanyEmployee
{
    public int Id { get; set; }

    public string FullName { get; set; }
    public string JobType { get; set; }

    [ForeignKey("CompanyProfile")]
    public int CompanyProfileId { get; set; }
    public CompanyProfile CompanyProfile { get; set; } = null!;

    [ForeignKey("UserProfileId")]
    public int UserProfileId { get; set; }
    public UserProfile UserProfile { get; set; }
}
