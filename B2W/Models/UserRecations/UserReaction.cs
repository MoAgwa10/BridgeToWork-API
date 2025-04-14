using B2W.Models.Authentication;
using B2W.Models.Userpost;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace B2W.Models.UserRecations
{
    public class UserReaction
    {
        [Key]
        public int ReactionId { get; set; }

        public string UserId { get; set; }

        public int? PostId { get; set; }

        public int ReactionTypeId { get; set; }

        public DateTime CreatedAt { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        [ForeignKey("PostId")]
        public virtual Post Post { get; set; } = null!;

        [ForeignKey("ReactionTypeId")]
        public virtual ReactionType ReactionType { get; set; } = null!;
    }
}
