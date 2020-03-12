using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AspCoreGraphQL.Entities.Context;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using HotChocolate;
using HotChocolate.AspNetCore;
using AspCoreGraphQL.GraphQL;
using AspCoreGraphQL.GraphQL.Resolvers;
using AspCoreGraphQL.Entities;
using HotChocolate.Execution.Configuration;
using AspCoreGraphQL_Demo.GraphQL.Types;

namespace AspCoreGraphQL
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(b => b.UseSqlite(Program.DbConnection).EnableSensitiveDataLogging());
            //this is transient, so that we can create a new one in every field resolver
            services.AddDbContext<GraphQLDataContext>(b => b.UseSqlite(Program.DbConnection).EnableSensitiveDataLogging(), ServiceLifetime.Transient, ServiceLifetime.Transient);
            services.AddHttpContextAccessor();


            services.AddControllers()
            .AddNewtonsoftJson(o => o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddDataLoaderRegistry();
            services.AddGraphQL(sp => SchemaBuilder.New()
                                                   .AddServices(sp)
                                                   .AddQueryType<Query>()
            .AddType<PostType>()
            .AddType<Comment>()
            .AddType<PostResolvers>()
            .AddType<TagResolvers>()
                                                   .Create(), new QueryExecutionOptions
                                                   {
                                                       IncludeExceptionDetails = true,
                                                   });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<GraphQLDataContextMiddleware>();
            app.UseGraphQL("/graphql");
            app.UsePlayground("/graphql", "/graphql/playground");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
