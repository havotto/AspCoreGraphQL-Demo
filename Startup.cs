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
using System.Diagnostics;
using AspCoreGraphQL.GraphQL.Middlewares;
using AspCoreGraphQL.GraphQL.Directives;
using AspCoreGraphQL_Demo.Dto;
using Microsoft.AspNetCore.Authorization;
using AspCoreGraphQL_Demo.GraphQL.Mutations;
using HotChocolate.Resolvers;

namespace AspCoreGraphQL
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureAuthenticationServices(services);

            services.AddDbContext<DataContext>(b => b.UseSqlite(Program.DbConnection).EnableSensitiveDataLogging());
            //this is transient, so that we can create a new one in every field resolver
            services.AddDbContext<GraphQLDataContext>(b => b.UseSqlite(Program.DbConnection).EnableSensitiveDataLogging(), ServiceLifetime.Transient, ServiceLifetime.Transient);
            services.AddHttpContextAccessor();


            services.AddControllers()
            .AddNewtonsoftJson(o => o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddAuthorization(config =>
            {
                var defaultPolicy = new AuthorizationPolicyBuilder().RequireAssertion(context =>
                {
                    var directiveContext = context.Resource as IDirectiveContext;
                    Console.WriteLine("DEFAULT POLICY EVAL: " + directiveContext?.Path ?? "NULL");
                    return true;
                }).Build();
                config.DefaultPolicy = defaultPolicy;

                config.AddPolicy("CanSeeRatings", b =>
                {
                    //this is evaluated for every post rating field
                    b.RequireAssertion(context =>
                    {
                        var directiveContext = context.Resource as IDirectiveContext;
                        Console.WriteLine("CanSeeRatings POLICY EVAL: " + directiveContext?.Path ?? "NULL");
                        return true;
                    });
                });
            });

            services.AddDataLoaderRegistry();
            services.AddGraphQL(sp => SchemaBuilder.New()
                .ModifyOptions(o => o.UseXmlDocumentation = true)
                .AddServices(sp)
                .AddQueryType<Query>()
                .AddType<PostType>()
                .AddType<PostResolvers>()
                .AddType<TagResolvers>()
                .AddType<CommentResolvers>()
                .AddMutationType(d => d.Name("Mutation"))
                .AddType<UserMutations>()
                .AddDirectiveType<AppenderDirectiveType>()
                .AddAuthorizeDirectiveType()
                //.Use<ToUpperMiddleware>()
                .Use(next => context =>
                {
                    //inline global field middleware
                    return next(context);
                })
                .Create(), new QueryExecutionOptions
                {
                    IncludeExceptionDetails = true,
                    //set http header to trace: {"GraphQL-Tracing": 1}
                    TracingPreference = TracingPreference.OnDemand,
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

            app.UseAuthentication();
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
