using System.Text.Json.Serialization;

namespace StackOverflowAPI.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Address Address { get; set; }

        [JsonIgnore]
        public List<Rating> Ratings { get; set; } = new List<Rating>();

        public List<Question> Questions { get; set; } = new List<Question>();
        public List<Answer> Answers { get; set; } = new List<Answer>();

        [JsonIgnore]
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}