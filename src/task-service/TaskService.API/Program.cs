using Common.Logging;
using Common.Extensions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using TaskService.API.Extensions;
using Common.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.UseCommonSerilog();
builder.RegisterServices();
builder.Services.AddServiceHealthChecks(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseGlobalExceptionHandling();

app.UseAuthorization();
    
app.UseCommonHttpObservabilityAndHealthCheck();

app.MapControllers();

app.Run();
