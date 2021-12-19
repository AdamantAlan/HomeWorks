using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Filters;
using Microsoft.Extensions.Configuration;
using Data.Interfaces;
using Data.Repositories;
using Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Microsoft.OpenApi.Models;
using System.IO;
using Filters.Schemas;
using Data;
using MassTransit;
using Data.Data.MessageMQ;
using TransactionManager.Services;

namespace TransactionManager
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(opt =>
            {
                opt.EnableEndpointRouting = false;
                opt.Filters.Add(typeof(LoggingFilter));
                opt.Filters.Add(typeof(ExceptionFilter));
            });

            services.AddHttpContextAccessor();
            services.AddHttpClient<IHttpClientForServiceCardManager, HttpClientForServiceCardManager>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IRepository, RepositoryDbHomeWorkBase>();
            services.AddHttpClient<IHttpClientForServiceCardManager, HttpClientForServiceCardManager>(client => 
            {
                client.BaseAddress = new Uri("http://localhost:12635");
                client.DefaultRequestHeaders.Add("API-KEY", "test");
            });

            services.AddDbContext<HomeWorkDbContext>(opt => opt.UseNpgsql(_config.GetConnectionString("Npg")));
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });

                x.AddRequestClient<NewCardMessageV1>();
            });

            services.AddMassTransitHostedService();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Transaction manager API", Version = "v1" });

                var filePath = Path.Combine(AppContext.BaseDirectory, $"{nameof(TransactionManager)}.xml");
                c.IncludeXmlComments(filePath);

                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Name = "API-KEY",
                    Description = "Api key auth",
                });

                var key = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "ApiKey"
                    },
                    In = ParameterLocation.Header
                };
                var requirement = new OpenApiSecurityRequirement
                {
                    { key, new List<string>() }
                };
                c.AddSecurityRequirement(requirement);

                c.SchemaFilter<EnumSchemaFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Transaction manager API V1");
            });

            app.UseReDoc(c => { c.RoutePrefix = "docs"; });

            app.Use(async (context, next) =>
            {
                if (!context.Request.Headers.TryGetValue("API-KEY", out var key) || !key.Equals("test"))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsJsonAsync(new ResultApi<string> { Result = "use header API-KEY:test !", ErrorCode = 401, ErrorMessage = "not authorized request" });
                    return;
                }

                await next();
            });

            app.Use((context, next) =>
            {
                context.Request.EnableBuffering();
                return next();
            });

            app.UseRouting();
            app.UseMvc();
        }
    }
}
