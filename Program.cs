using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspCoreGraphQL.Entities.Context;
using AspCoreGraphQL.Entities.Seed;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AspCoreGraphQL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DbConnection = new SqliteConnection("DataSource=:memory:");
            using (DbConnection)
            {
                DbConnection.Open();

                var host = CreateHostBuilder(args).Build();
                
                var scopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();
                using (var scope = scopeFactory.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
                    db.Database.EnsureCreated();
                    var posts = SeedData.Posts;
                    var comments = SeedData.Comment;
                    db.AddRange(posts);
                    db.AddRange(comments);
                    db.AddRange(SeedData.PostTags);
                    db.AddRange(SeedData.Tags);
                    db.SaveChanges();
                }

                host.Run();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static SqliteConnection DbConnection;

    }
}
