using System.ComponentModel.DataAnnotations.Schema;
using B2W.Models.Authentication;

namespace B2W.Models.User
{
    public class Experience
    {
        public int Id { get; set; }
        public string JobTitle { get; set; }
        public string OrganizationName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser User { get; set; }
    }
}

