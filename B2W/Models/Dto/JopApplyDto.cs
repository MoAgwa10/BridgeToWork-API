namespace B2W.Models.Dto
{
    public class JopApplyDto
    {
        public int JopId { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? AnyComment { get; set; }

    }
}
