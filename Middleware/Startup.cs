using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Middleware.Data;
using Middleware.Extensions;
using Middleware.Interfaces;
using Middleware.Services;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Middleware
{
    // Ќе успеваю добавить интересные фичи и проверки, поэтому есть не красивые моменты.
    // Ќадеюсь расписал достаточно, чтобы показать, что разбираюсь в теме.
    // PS не стал в лог записывать e.Message and e.Trace, дл€ нагл€дности не большое предложени€.
    // ≈сли что-то нужно исправить пишите!

    public class Startup
    {

        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSingleton<ICardManager, CardManager>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> log, ICardManager manager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Logging(new ServerInfo { env = _config["ASPNETCORE_ENVIRONMENT"] ?? "No set", vs = _config["VisualStudioEdition"] ?? "No set", server = _config["LOGONSERVER"] ?? "No set" });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                // Test
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("OK!");
                });

                #region SetCard

                // «апись карт пользовател€.
                // POST http://localhost:12635/SetCard
                // {"Cvc":425,"Pan":1234567399,"Expire":"2021-11-18T12:04:53+05:00","Name":"Myagkov Dmitriy","IsDefault":true,"UserId":456}
                endpoints.MapPost("/SetCard", async context =>
                {
                    Card card = new();
                    try
                    {
                        using var streamReader = new StreamReader(context.Request.Body);
                        string bodyContent = await streamReader.ReadToEndAsync();

                        card = JsonSerializer.Deserialize<Card>(bodyContent);
                        var cardWriteDto = manager.SetCard(card);

                        log.LogInformation($"Set new Card, user:{card.UserId}");
                        ResultApi result = new() { Result = cardWriteDto, ErrorCode = 0, ErrorMessage = null };
                        context.Response.StatusCode = 200;

                        await context.Response.WriteAsJsonAsync(result);
                    }
                    catch (JsonException je)
                    {
                        context.Response.StatusCode = 500;
                        log.LogError($"Error set new Card!");
                        ResultApi result = new() { Result = null, ErrorCode = 100, ErrorMessage = je.Message };
                        await context.Response.WriteAsJsonAsync(result);
                    }
                    catch (Exception e)
                    {
                        context.Response.StatusCode = 500;
                        log.LogError($"Error set new Card!");
                        ResultApi result = new() { Result = null, ErrorCode = 500, ErrorMessage = e.Message };
                        await context.Response.WriteAsJsonAsync(result);
                    }
                });

                #endregion

                #region GetCard

                // „тение карт пользовател€.
                // GET http://localhost:12635/GetCard/{userId}
                endpoints.MapGet("/GetCard/{id}", async context =>
                {
                    try
                    {
                        var userId = long.Parse(context.Request.RouteValues["id"].ToString());
                        var cards = manager.GetCards(userId);

                        //Ћучше сделать проверку на единственность isDefault и свое исключение, не успеваю:(
                        if (cards.Count() == 0)
                        {
                            throw new ArgumentNullException("Cards not found.");
                        }

                        ResultApi result = new() { Result = cards, ErrorCode = 0, ErrorMessage = null };
                        log.LogInformation($"Get cards, user:{userId}");
                        context.Response.StatusCode = 200;
                        await context.Response.WriteAsJsonAsync(result);
                    }
                    catch (ArgumentNullException e)
                    {
                        context.Response.StatusCode = 404;
                        log.LogWarning($"Cards not found!");
                        ResultApi result = new() { Result = null, ErrorCode = 40467, ErrorMessage = e.Message };
                        await context.Response.WriteAsJsonAsync(result);
                    }
                    catch (Exception e)
                    {
                        context.Response.StatusCode = 500;
                        log.LogError($"Error for found cards!");
                        ResultApi result = new() { Result = null, ErrorCode = 999, ErrorMessage = e.Message };
                        await context.Response.WriteAsJsonAsync(result);
                    }
                });

                #endregion

            });
        }
    }
}
