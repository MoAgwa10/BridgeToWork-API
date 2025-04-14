namespace B2W.Models.Dto
{
    public class EducationDto
    {
        public int Id { get; set; }
        public string University { get; set; }
        public string Faculty { get; set; }
        public string Degree { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ApplicationUserId { get; set; }

    }
}
