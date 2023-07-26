using Microsoft.EntityFrameworkCore;

namespace ToDoMinimalAPI.Context
{
    public class ApiContext : DbContext
    {

        public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }

        public DbSet<ToDo> ToDos { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(builder =>
            {
                builder.HasMany(u => u.ToDos)
                .WithOne()
                .HasForeignKey(t => t.UserId);
            });
        }
    }
}
