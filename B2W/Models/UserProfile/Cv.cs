using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace B2W.Models.User
{
    public class Cv
    {
        [Key]
        public int Id { get; set; }

        public string CvFilePath { get; set; } // أو CvUrl حسب انت هتخزنه ازاي

        [ForeignKey("UserProfileId")]
        public int UserProfileId { get; set; }

        public UserProfile UserProfile { get; set; }
    }
}
