namespace B2W.Dto.UserProfileDtos
{
    public class UserCertificationDto
    {
        public int CertificationId { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int UserProfileId { get; set; }
    }
}
