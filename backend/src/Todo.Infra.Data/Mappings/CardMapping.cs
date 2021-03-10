using Todo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Todo.Infra.Data.Mappings
{
    public class CardMapping : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.ToTable("Cards");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .HasColumnName("Title")
                .IsRequired();

            builder.Property(x => x.Priority)
                .HasColumnName("Priority")
                .IsRequired();
        }
    }
}
