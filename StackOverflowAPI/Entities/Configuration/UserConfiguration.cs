using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StackOverflowAPI.Entities.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasOne(u => u.Address).WithOne(a => a.Author).HasForeignKey<Address>(a => a.AuthorId);
            builder.HasMany(u => u.Answers).WithOne(a => a.Author).HasForeignKey(a => a.AuthorId).OnDelete(DeleteBehavior.ClientCascade);
            builder.HasMany(u => u.Questions).WithOne(q => q.Author).HasForeignKey(q => q.AuthorId).OnDelete(DeleteBehavior.ClientCascade); ;
            builder.HasMany(u => u.Comments).WithOne(c => c.Author).HasForeignKey(c => c.AuthorId).OnDelete(DeleteBehavior.ClientCascade);
            builder.HasMany(u => u.Ratings).WithOne(r => r.User).HasForeignKey(r => r.UserId);
        }
    }
}