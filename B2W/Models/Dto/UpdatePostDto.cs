namespace B2W.Models.Dto
{
    public class UpdatePostDto
    {
        public string Content { get; set; } = null!;
        public IFormFile? Image { get; set; }

    }
}
