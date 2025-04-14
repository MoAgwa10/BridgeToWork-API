using B2W.Models.Authentication;
using B2W.Models.UserComment;
using System.ComponentModel.DataAnnotations.Schema;

namespace B2W.Models.Userpost
{
    public class Post
    {
        public int PostId { get; set; }

        public string UserId { get; set; } = null!;  

        public string? Content { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public int LikesCount { get; set; } = 0;

        public int LoveCount { get; set; } = 0;

        public int DisLikeCount { get; set; } = 0;

        public int HahaCount { get; set; } = 0;

        public int WowCount { get; set; } = 0;

        public int SadCount { get; set; } = 0;

        public int AngryCount { get; set; } = 0;

        public string? Image { get; set; }

        // تعريف العلاقة بين المنشورات والمستخدمين

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        public virtual ICollection<Comment> Comment { get; set; } = new List<Comment>();

    }
}
