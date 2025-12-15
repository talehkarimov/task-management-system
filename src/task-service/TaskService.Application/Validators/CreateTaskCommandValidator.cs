using FluentValidation;
using TaskService.Application.Commands;

namespace TaskService.Application.Validators;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(2000);
        RuleFor(x => x.ReporterUserId).NotEmpty();
        RuleFor(x => x.DueDate).GreaterThan(DateTime.Now).When(x => x.DueDate.HasValue);
    }
}
