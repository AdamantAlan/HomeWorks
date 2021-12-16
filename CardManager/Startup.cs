using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Data;
using Data.DbContexts;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using CardManager.Filters;
using Data.Repositories;
using CardManager.Filters.Schemas;
using CardManager.Extensions;
using MassTransit;
using CardManager.Service.Consumers;

namespace CardManager
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

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IRepository, RepositoryDbHomeWorkBase>();
            services.AddDbContext<HomeWorkDbContext>(opt => opt.UseNpgsql(_config.GetConnectionString("Npg")));
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

            services.AddMassTransit(x =>
            {
                x.AddConsumer<NewCardCreateConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });
            });

            services.AddMassTransitHostedService();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Card manager API", Version = "v1" });

                var filePath = Path.Combine(AppContext.BaseDirectory, $"{nameof(CardManager)}.xml");
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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.Logging(new ServerInfo { env = _config["ASPNETCORE_ENVIRONMENT"] ?? "No set", vs = _config["VisualStudioEdition"] ?? "No set", server = _config["LOGONSERVER"] ?? "No set" });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Card manager API V1");
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