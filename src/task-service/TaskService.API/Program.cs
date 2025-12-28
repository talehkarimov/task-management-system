using Common.Logging;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using TaskService.API.Extensions;
using TaskService.API.Health;
using TaskService.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.UseCommonSerilog();
builder.RegisterServices();
builder.Services.AddServiceHealthChecks(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseGlobalExceptionHandling();

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains(HealthCheckTags.Live)
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains(HealthCheckTags.Ready)
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
