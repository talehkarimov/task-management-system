using FluentValidation;
using TaskService.Application.Commands;
using TaskService.Application.Validators;

namespace TaskService.API.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssemblyContaining<CreateTaskCommand>());

            builder.Services.AddValidatorsFromAssemblyContaining<CreateTaskCommandValidator>();
            return builder;
        }
    }
}
