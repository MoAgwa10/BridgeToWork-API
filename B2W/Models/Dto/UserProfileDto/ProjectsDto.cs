using B2W.Models.User;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace B2W.Dto.UserProfileDtos
{
    public class ProjectsDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)] // ممكن تزود أو تقلل حسب المطلوب
        public string Title { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        public int UserProfileId { get; set; }


    }
}
