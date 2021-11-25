using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Middleware.Data;
using Middleware.Extensions;
using Middleware.Filters;
using Middleware.Interfaces;
using Middleware.Services;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Middleware
{
    // �� ������� �������� ���������� ���� � ��������, ������� ���� �� �������� �������.
    // ������� �������� ����������, ����� ��������, ��� ���������� � ����.
    // PS �� ���� � ��� ���������� e.Message and e.Trace, ��� ����������� �� ������� �����������.
    // ���� ���-�� ����� ��������� ������!

    public class Startup
    {

        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(opt => { opt.EnableEndpointRouting = false; opt.Filters.Add(typeof(LoggingFilter)); });
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSingleton<ICardManager, CardManager>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Card manager API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Logging(new ServerInfo { env = _config["ASPNETCORE_ENVIRONMENT"] ?? "No set", vs = _config["VisualStudioEdition"] ?? "No set", server = _config["LOGONSERVER"] ?? "No set" });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Card manager API V1");
            });

            app.UseRouting();
            app.UseMvc();
        }
    }
}
