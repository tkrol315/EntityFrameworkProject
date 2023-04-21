using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StackOverflowAPI.Entities.Configuration
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasMany(q => q.Comments).WithOne(c => c.Question).HasForeignKey(c => c.QuestionId);
            builder.HasMany(q => q.Answers).WithOne(a => a.Question).HasForeignKey(a => a.QuestionId).OnDelete(DeleteBehavior.ClientCascade);
            builder.HasMany(q => q.Tags).WithMany(t => t.Questions);
        }
    }
}