using B2W.Models.User;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

 namespace B2W.Dto.UserProfileDtos
{
    public class MillStoneDto
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Company { get; set; }

        public DateTime Date { get; set; }


        public int UserProfileId { get; set; }


    }
}
