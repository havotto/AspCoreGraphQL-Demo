using System.Threading;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AspCoreGraphQL.Entities.Context
{
    public class DataContext : DbContext
    {
        static long idCounter = 0;
        long myId;
        private readonly ILogger<DataContext> logger;

        public DataContext(DbContextOptions<DataContext> options, ILogger<DataContext> logger) : base(options)
        {
            myId = Interlocked.Increment(ref idCounter);
            this.logger = logger;
            logger.LogInformation("CREATE DB " + myId);
        }

        public override void Dispose()
        {
            logger.LogInformation("DISPOSE DB " + myId);
            base.Dispose();
        }

        public override ValueTask DisposeAsync()
        {
            logger.LogInformation("DISPOSE ASYNC DB " + myId);
            return base.DisposeAsync();
        }

#nullable disable
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<PostTag> PostTags => Set<PostTag>();
#nullable restore
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