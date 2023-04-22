using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StackOverflowAPI.Entities.Configuration
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasData(
                new Tag() { Id = 1, Name = "C#" },
                new Tag() { Id = 2, Name = "C++" },
                new Tag() { Id = 3, Name = "Java" }
                );
        }
    }
}