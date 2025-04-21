using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using B2W.Models.User;

public class MillStones
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    public string Company { get; set; }

    public DateTime Date { get; set; }

    // Foreign Key
    [ForeignKey("UserProfileId")]
    public int UserProfileId { get; set; }
   
    public UserProfile UserProfile { get; set; }
}