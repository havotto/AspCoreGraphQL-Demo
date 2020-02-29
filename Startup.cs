using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using AspCoreGraphQL.GQL.GqlSchema;
using AspCoreGraphQL.Entities.Context;
using AspCoreGraphQL.GQL;

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

            services.AddScoped<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));
            services.AddScoped<AppSchema>();
            services.AddGraphQL(options => options.ExposeExceptions = false)
            .AddGraphTypes(ServiceLifetime.Scoped)
            .AddDataLoader()
            .AddValueConverters();

            services.AddControllers(o => o.EnableEndpointRouting = false)
            .AddNewtonsoftJson(o => o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            // GraphQL middleware reads requeststream synchronously:
            //   at Newtonsoft.Json.JsonSerializer.Deserialize[T](JsonReader reader)
            //   at GraphQL.Server.Transports.AspNetCore.GraphQLHttpMiddleware`1.Deserialize[T](Stream s)
            //   at GraphQL.Server.Transports.AspNetCore.GraphQLHttpMiddleware`1.InvokeAsync(HttpContext context)
            services.Configure<KestrelServerOptions>(options => options.AllowSynchronousIO = true);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            /*
                        app.UseRouting();
                        app.UseAuthorization();
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                        });
            /**/
            app.UseGraphQL<AppSchema>();
            app.UseGraphQLPlayground(options: new GraphQLPlaygroundOptions());
            app.UseMvc();
        }
    }


}
