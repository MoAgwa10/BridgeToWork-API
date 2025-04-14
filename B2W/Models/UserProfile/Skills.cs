using System.ComponentModel.DataAnnotations.Schema;
using B2W.Models.Authentication;

namespace B2W.Models.User
{
    public class Skills
    {

        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser applicationUser { get; set; }















    }
}
