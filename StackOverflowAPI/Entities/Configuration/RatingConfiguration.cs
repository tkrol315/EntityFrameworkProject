using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StackOverflowAPI.Entities.Configuration
{
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.HasOne(r => r.User).WithMany(u => u.Ratings).HasForeignKey(r => r.UserId);
            builder.HasOne(r => r.Question).WithMany(q => q.Ratings).HasForeignKey(r => r.QuestionId);
            builder.HasOne(r => r.Answer).WithMany(a => a.Ratings).HasForeignKey(r => r.AnswerId);
        }
    }
}