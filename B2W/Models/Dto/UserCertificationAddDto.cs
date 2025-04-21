namespace B2W.Models.Dto
{
    public class UserCertificationAddDto
    {
        public int UserProfileId { get; set; }

        public string Description { get; set; } = null!;
        public IFormFile? Image { get; set; }
    }
}
