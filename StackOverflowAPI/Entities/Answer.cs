using System.Text.Json.Serialization;

namespace StackOverflowAPI.Entities
{
    public class Answer
    {
        public Guid Id { get; set; }

        [JsonIgnore]
        public Author Author { get; set; }

        public Guid AuthorId { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }

        [JsonIgnore]
        public Question Question { get; set; }

        public Guid QuestionId { get; set; }

        [JsonIgnore]
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}