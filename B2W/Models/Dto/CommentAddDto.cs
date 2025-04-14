namespace B2W.Models.Dto
{
    public class CommentAddDto:CommentEditDto
    {
        public int PostId { get; set; }
        public string UserId { get; set; }
    }
}
