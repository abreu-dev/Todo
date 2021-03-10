using Todo.Domain.Entities;
using Todo.Domain.Interfaces;
using Todo.Infra.Data.Mappings;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Todo.Infra.Data
{
    public class TodoContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IUnitOfWork
    {
        public DbSet<Board> Boards { get; set; }
        public DbSet<Column> Columns { get; set; }
        public DbSet<Card> Cards { get; set; }

        public TodoContext(DbContextOptions<TodoContext> options) : base(options) 
        {
        }

        public async Task<bool> Commit()
        {
            return await SaveChangesAsync() > 0;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BoardMapping());
            modelBuilder.ApplyConfiguration(new ColumnMapping());
            modelBuilder.ApplyConfiguration(new CardMapping());

            base.OnModelCreating(modelBuilder);
        }
    }
}
