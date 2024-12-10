using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Gerekli servisleri ekleyin (Varsa)
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        // Middleware: IP Kontrolü
        app.Use(async (context, next) =>
        {
            var allowedIPs = new[] { "127.0.0.1", "::1" };
            var clientIP = context.Connection.RemoteIpAddress?.ToString();

            if (!allowedIPs.Contains(clientIP))
            {
                context.Response.StatusCode = 403; // Forbidden
                await context.Response.WriteAsync("IP adresine izin verilmiyor.");
                return;
            }

            await next.Invoke();
        });

        // Middleware: Kimlik Doğrulama
        app.Use(async (context, next) =>
        {
            var username = context.Request.Headers["KullaniciAdi"].ToString();
            var password = context.Request.Headers["Parola"].ToString();

            if (username != "testUser" || password != "securePassword")
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Kimlik doğrulama başarısız.");
                return;
            }

            await next.Invoke();
        });

        // Rota Tanımlamaları
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/{participantCode}/data", async context =>
            {
                var participantCode = context.Request.RouteValues["participantCode"]?.ToString();

                if (participantCode != "12345")
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    await context.Response.WriteAsync("Yetkilendirme başarısız.");
                    return;
                }

                await context.Response.WriteAsync("Veri başarıyla alındı.");
            });
        });
    }
}
