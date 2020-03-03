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
using Microsoft.Extensions.Logging;
using AspCoreGraphQL.Entities.Context;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using HotChocolate;
using HotChocolate.AspNetCore;
using AspCoreGraphQL.GraphQL;

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
            services.AddControllers()
            .AddNewtonsoftJson(o=>o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddGraphQL(sp => SchemaBuilder.New()
                                                   .AddServices(sp)
                                                   .AddQueryType<Query>()
                                                   .Create());
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

            app.UseGraphQL("/graphql");
            app.UsePlayground("/graphql", "/graphql/playground");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
