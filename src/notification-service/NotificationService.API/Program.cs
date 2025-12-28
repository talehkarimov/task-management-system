using Common.Logging;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using NotificationService.API.Health;
using NotificationService.API.Middlewares;
using NotificationService.Application;
using NotificationService.Application.Commands;
using NotificationService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.UseCommonSerilog();

builder.Services.AddHttpContextAccessor();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<UpdateNotificationPreferencesCommand>());

builder.Services.AddApplicationServices();

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddServiceHealthChecks(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains(HealthCheckTags.Live)
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains(HealthCheckTags.Ready)
});

app.UseAuthorization();

app.MapControllers();

app.Run();
