using B2W.Models.Authentication;
using B2W.Models.Userpost;
using System.ComponentModel.DataAnnotations.Schema;

namespace B2W.Models.UserComment
{
    public class Comment
    {
        public int CommentId { get; set; }

        public int PostId { get; set; }

        public string? UserId { get; set; } = null!; // تغيير النوع من int إلى string

        public string CommentText { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [ForeignKey("PostId")]
        public virtual Post Post { get; set; } = null!; // تصحيح اسم الخاصية

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; } = null!; // تصحيح اسم الخاصية
    }
}
