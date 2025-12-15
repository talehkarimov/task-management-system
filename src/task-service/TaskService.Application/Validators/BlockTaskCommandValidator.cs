using FluentValidation;
using TaskService.Application.Commands;

namespace TaskService.Application.Validators;

public class BlockTaskCommandValidator : AbstractValidator<BlockTaskCommand>
{
    public BlockTaskCommandValidator()
    {
        RuleFor(x => x.TaskId).NotEmpty();
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(500);
        RuleFor(x => x.ChangedByUserId).NotEmpty();
    }
}
