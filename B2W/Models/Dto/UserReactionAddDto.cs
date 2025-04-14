namespace B2W.Models.Dto
{
    public class UserReactionAddDto: UserReactionEditDto
    {
        public string UserId { get; set; }

        public int? PostId { get; set; }
    }
}
