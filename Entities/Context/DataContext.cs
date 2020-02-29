using Microsoft.EntityFrameworkCore;

namespace AspCoreGraphQL.Entities.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; private set; }
        public DbSet<Comment> Comments { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>(e =>
            {
                e.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Comment>(e =>
            {
                e.HasKey(e => e.Id);
                e.HasOne<Post>().WithMany().HasForeignKey(nameof(Comment.PostId)).OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}