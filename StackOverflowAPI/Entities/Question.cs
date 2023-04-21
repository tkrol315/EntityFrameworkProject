namespace StackOverflowAPI.Entities
{
    public class Question
    {
        public Guid Id { get; set; }
        public Author Author { get; set; }
        public Guid AuthorId { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Answer> Answers { get; set; } = new List<Answer>();
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}