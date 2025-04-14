namespace B2W.Models.Dto
{
    public class UserCertificationAddDto
    {
        public string UserId { get; set; } = null!;
        public string Description { get; set; } = null!;
        public IFormFile? Image { get; set; }
    }
}
