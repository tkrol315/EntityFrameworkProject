using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StackOverflowAPI.Entities.Configuration
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasOne(a => a.Address).WithOne(a => a.Author).HasForeignKey<Address>(a => a.AuthorId);
            builder.HasMany(a => a.Answers).WithOne(a => a.Author).HasForeignKey(a => a.AuthorId);
            builder.HasMany(a => a.Questions).WithOne(q => q.Author).HasForeignKey(q => q.AuthorId);
            builder.HasMany(a => a.Comments).WithOne(c => c.Author).HasForeignKey(c => c.AuthorId).OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}