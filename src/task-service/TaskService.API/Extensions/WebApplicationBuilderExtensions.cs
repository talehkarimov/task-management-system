using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TaskService.Application.Behaviors;
using TaskService.Application.Commands;
using TaskService.Application.Interfaces;
using TaskService.Application.Validators;
using TaskService.Infrastructure.Caching;
using TaskService.Infrastructure.Idempotency;
using TaskService.Infrastructure.Outbox;
using TaskService.Infrastructure.Persistence;
using TaskService.Infrastructure.Repositories;

namespace TaskService.API.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddHealthChecks()
                .AddDbContextCheck<TaskDbContext>(
                    name: "db")
                .AddCheck<TaskService.Infrastructure.Health.OutboxHealthCheck>(
                    name: "outbox");
            builder.Services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri(
                                "amqps://fxfusnbm:qhlEeivcfYNni6tk5NFDN8Vsq1Kn18sE@campbell.lmq.cloudamqp.com/fxfusnbm"));
                });
            });

            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssemblyContaining<CreateTaskCommand>());

            builder.Services.AddValidatorsFromAssemblyContaining<CreateTaskCommandValidator>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            builder.Services.AddDbContext<TaskDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<ITaskRepository, TaskRepository>();
            builder.Services.AddScoped<ITaskCommentRepository, TaskCommentRepository>();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<IIdempotencyService, IdempotencyService>();

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>),typeof(IdempotencyBehavior<,>));

            builder.Services.AddScoped<ITaskReadDbContext>(provider => provider.GetRequiredService<TaskDbContext>());

            builder.Services.AddMemoryCache();
            builder.Services.AddScoped<ICacheService, InMemoryCacheService>();

            builder.Services.AddScoped<IDomainEventDispatcher, OutboxDomainEventDispatcher>();
            builder.Services.AddHostedService<OutboxProcessor>();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IRequestContext, Context.HttpRequestContext>();


            return builder;
        }
        public static WebApplicationBuilder RegisterSerilog(this WebApplicationBuilder builder)
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
}
