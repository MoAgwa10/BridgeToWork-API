using B2W.Models.Dto.UserProfileDto;

namespace B2W.Dto.UserProfileDtos
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string JobTitle { get; set; }
        public string JobType { get; set; }
        public string WorkModel { get; set; }
        public string ExperienceLevel { get; set; }
        public string DesiredJobTitle { get; set; }
        public string DisabilityType { get; set; }
        public string FontSize { get; set; }
        public bool DarkMode { get; set; }
        public string ApplicationUserId { get; set; }

        public List<SkillsDto> Skills { get; set; }
        public List<EducationDto> Educations { get; set; }
        public List<ExperienceDto> Experiences { get; set; }
        public List<ProjectsDto> Projects { get; set; }
        public List<MillStoneDto> MillStones { get; set; }
        public List<UserCertificationDto> UserCertifications { get; set; }
        public UserProfilePictureDto UserProfilePicture { get; set; }
        public CvDto Cv { get; set; }
    }
}
