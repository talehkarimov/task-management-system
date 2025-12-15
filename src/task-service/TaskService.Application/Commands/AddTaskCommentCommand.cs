using MediatR;

namespace TaskService.Application.Commands;

public record AddTaskCommentCommand(Guid TaskId,
    Guid UserId,
    string Content) : IRequest;
