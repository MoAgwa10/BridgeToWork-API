namespace B2W.Models.Dto
{
    public class JopDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Requirments { get; set; } = string.Empty;
        public string AboutCompany { get; set; } = string.Empty;
        public string JopType { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string WorkingModel { get; set; } = string.Empty;
        public decimal? Salary { get; set; }
    }
}
