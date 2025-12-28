using Microsoft.AspNetCore.Builder;
using Serilog;
namespace Common.Logging;

public static class LoggingExtensions
{
    public static WebApplicationBuilder UseCommonSerilog(
        this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithProperty(
                    "ServiceName",
                    context.HostingEnvironment.ApplicationName);
        });

        return builder;
    }
}