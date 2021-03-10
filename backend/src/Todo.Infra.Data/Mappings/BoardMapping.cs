using Todo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Todo.Infra.Data.Mappings
{
    public class BoardMapping : IEntityTypeConfiguration<Board>
    {
        public void Configure(EntityTypeBuilder<Board> builder)
        {
            builder.ToTable("Boards");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .HasColumnName("Title")
                .IsRequired();

            builder.HasMany(x => x.Columns)
                .WithOne(y => y.Board)
                .HasForeignKey(y => y.BoardId);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Boards)
                .HasForeignKey(x => x.UserId)
                .IsRequired();
        }
    }
}
