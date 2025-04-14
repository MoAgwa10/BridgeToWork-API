namespace B2W.Models.Dto
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string JobTitle { get; set; }


        public string ProfileImageUrl { get; set; }

        public string JobType { get; set; }
        public string WorkModel { get; set; }
        public string ExperienceLevel { get; set; }
        public string DesiredJobTitle { get; set; }
        public string DisabilityType { get; set; }
        public string FontSize { get; set; }
        public bool DarkMode { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
