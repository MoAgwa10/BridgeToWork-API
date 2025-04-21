namespace B2W.Models.Dto.CompanyProfileDto
{
    public class CompanyReviewDto
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public int Rating { get; set; }

        public int CompanyProfileId { get; set; }

        public int UserProfileId { get; set; }
    }
}
