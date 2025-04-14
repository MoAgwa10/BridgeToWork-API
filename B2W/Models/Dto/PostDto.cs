namespace B2W.Models.Dto
{
    public class PostDto
    {   
        public string UserId { get; set; } = null!;
        public string Content { get; set; } = null!;
        public IFormFile? Image { get; set; }

    }
}
