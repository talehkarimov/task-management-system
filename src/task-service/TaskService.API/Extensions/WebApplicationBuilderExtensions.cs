using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TaskService.Application.Commands;
using TaskService.Application.Interfaces;
using TaskService.Application.Validators;
using TaskService.Infrastructure.Persistence;
using TaskService.Infrastructure.Repositories;

namespace TaskService.API.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssemblyContaining<CreateTaskCommand>());

            builder.Services.AddValidatorsFromAssemblyContaining<CreateTaskCommandValidator>();

            builder.Services.AddDbContext<TaskDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<ITaskRepository, TaskRepository>();
            builder.Services.AddScoped<ITaskCommentRepository, TaskCommentRepository>();

            return builder;
        }
    }
}
