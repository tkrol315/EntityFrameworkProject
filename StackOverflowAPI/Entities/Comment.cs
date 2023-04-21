namespace StackOverflowAPI.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public Question? Question { get; set; }
        public Guid QuestionId { get; set; }
        public Answer? Answer { get; set; }
        public Guid AnswerId { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
    }
}