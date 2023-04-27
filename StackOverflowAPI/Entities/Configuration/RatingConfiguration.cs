using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StackOverflowAPI.Entities.Configuration
{
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.HasOne(r => r.User).WithMany(u => u.Ratings).HasForeignKey(r => r.UserId);
            builder.HasOne(r => r.Answer).WithOne(a => a.Rating).HasForeignKey<Rating>(r => r.AnswerId);
            builder.HasOne(r => r.Question).WithOne(q => q.Rating).HasForeignKey<Rating>(r => r.QuestionId);
        }
    }
}