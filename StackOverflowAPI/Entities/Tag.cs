using System.Text.Json.Serialization;

namespace StackOverflowAPI.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}