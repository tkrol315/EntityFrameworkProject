using System.Text.Json.Serialization;

namespace StackOverflowAPI.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public Guid AuthorId { get; set; }

        [JsonIgnore]
        public Author Author { get; set; }
    }
}