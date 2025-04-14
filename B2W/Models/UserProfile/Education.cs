using System.ComponentModel.DataAnnotations.Schema;
using B2W.Models.Authentication;

namespace B2W.Models.User
{
    public class Education
    {

        public int Id { get; set; }
        public string University { get; set; }
        public string Faculty { get; set; }
        public string Degree { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

       
        public string ApplicationUserId { get; set; } 

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser User { get; set; } 
    }
}
