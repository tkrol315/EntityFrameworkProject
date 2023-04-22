using System.Text.Json.Serialization;

namespace StackOverflowAPI.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public Question? Question { get; set; }

        public Guid QuestionId { get; set; }

        public Answer? Answer { get; set; }

        public Guid AnswerId { get; set; }
        public Author Author { get; set; }
        public Guid AuthorId { get; set; }
        public string Content { get; set; }
    }
}