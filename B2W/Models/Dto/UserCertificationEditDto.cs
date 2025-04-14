namespace B2W.Models.Dto
{
    public class UserCertificationEditDto
    {
        public string Description { get; set; } = null!;
        public IFormFile? Image { get; set; }
    }
}
