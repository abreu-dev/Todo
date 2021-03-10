using Todo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Todo.Infra.Data.Mappings
{
    public class ColumnMapping : IEntityTypeConfiguration<Column>
    {
        public void Configure(EntityTypeBuilder<Column> builder)
        {
            builder.ToTable("Columns");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .HasColumnName("Title")
                .IsRequired();

            builder.Property(x => x.PositionInBoard)
                .HasColumnName("PositionInBoard")
                .IsRequired();

            builder.HasMany(x => x.Cards)
                .WithOne(y => y.Column)
                .HasForeignKey(y => y.ColumnId);
        }
    }
}
