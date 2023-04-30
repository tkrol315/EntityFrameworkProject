using StackOverflowAPI.Migrations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StackOverflowAPI.Entities
{
    public class Question
    {
        public Guid Id { get; set; }

        [JsonIgnore]
        public User Author { get; set; }

        public Guid AuthorId { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;

        [JsonIgnore]
        public List<Comment> Comments { get; set; } = new List<Comment>();

        public List<Answer> Answers { get; set; } = new List<Answer>();
        public List<Tag> Tags { get; set; } = new List<Tag>();

        [JsonIgnore]
        public List<Rating> Ratings { get; set; } = new List<Rating>();

        public int RatingSum { get; set; }
    }
}