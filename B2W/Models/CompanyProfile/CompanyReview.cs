using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using B2W.Models.CompanyProfile;
using B2W.Models.User;

public class CompanyReview
{
    [Key]
    public int Id { get; set; }

    public string Message { get; set; }

    public int Rating { get; set; }

    // ربط الريفيو بالشركة
    [ForeignKey("CompanyProfileId")]
    public int CompanyProfileId { get; set; }
    public CompanyProfile CompanyProfile { get; set; }

    // ربط الريفيو ببروفايل المستخدم
    [ForeignKey("UserProfileId")]
    public int UserProfileId { get; set; }
    public UserProfile UserProfile { get; set; }
}