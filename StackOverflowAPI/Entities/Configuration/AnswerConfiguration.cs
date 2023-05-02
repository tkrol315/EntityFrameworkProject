using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StackOverflowAPI.Entities.Configuration
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.HasMany(a => a.Comments).WithOne(c => c.Answer).HasForeignKey(c => c.AnswerId).OnDelete(DeleteBehavior.ClientCascade);
            builder.Property(a => a.RatingSum).HasColumnName("RatingSum");
        }
    }
}