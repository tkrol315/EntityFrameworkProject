using System.Text.Json.Serialization;

namespace StackOverflowAPI.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        public int Value { get; set; }

        public Guid UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        public Guid? QuestionId { get; set; }
        public Question? Question { get; set; }
        public Guid? AnswerId { get; set; }
        public Answer? Answer { get; set; }
    }
}