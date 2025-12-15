using FluentValidation;
using TaskService.Application.Commands;

namespace TaskService.Application.Validators;

public class AssignTaskCommandValidator : AbstractValidator<AssignTaskCommand>
{
    public AssignTaskCommandValidator()
    {
        RuleFor(x => x.TaskId).NotEmpty();
        RuleFor(x => x.AssigneeUserId).NotEmpty();
        RuleFor(x => x.ChangedByUserId).NotEmpty();
    }
}
