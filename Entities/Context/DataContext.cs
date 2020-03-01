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
        public DbSet<Tag> Tags { get; private set; }
        public DbSet<PostTag> PostTags { get; set; }

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

            modelBuilder.Entity<Tag>(e =>
            {
                e.HasKey(e => e.Id);
            });

            modelBuilder.Entity<PostTag>(e =>
            {
                e.HasKey(e => new { e.PostId, e.TagId });
                e.HasOne<Post>().WithMany().HasForeignKey(nameof(PostTag.PostId)).OnDelete(DeleteBehavior.Cascade);
                e.HasOne<Tag>().WithMany().HasForeignKey(nameof(PostTag.TagId)).OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}