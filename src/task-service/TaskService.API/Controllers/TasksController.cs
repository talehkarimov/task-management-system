using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskService.API.Models;
using TaskService.Application.Commands;

namespace TaskService.API.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateTaskRequest request,
        CancellationToken cancellationToken)
    {
        var taskId = await _mediator.Send(
            new CreateTaskCommand(
                request.ProjectId,
                request.Title,
                request.Description,
                request.ReporterUserId,
                request.AssigneeUserId,
                request.DueDate,
                request.Priority),
            cancellationToken);

        return CreatedAtAction(nameof(Create), new { id = taskId }, taskId);
    }

    [HttpPut("{id:guid}/assign")]
    public async Task<IActionResult> Assign(
        Guid id,
        [FromBody] AssignTaskRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new AssignTaskCommand(
                id,
                request.AssigneeUserId,
                request.ChangedByUserId),
            cancellationToken);

        return NoContent();
    }

    [HttpPut("{id:guid}/status")]
    public async Task<IActionResult> ChangeStatus(
        Guid id,
        [FromBody] ChangeTaskStatusRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new ChangeTaskStatusCommand(
                id,
                request.NewStatus,
                request.ChangedByUserId),
            cancellationToken);

        return NoContent();
    }

    [HttpPut("{id:guid}/block")]
    public async Task<IActionResult> Block(
        Guid id,
        [FromBody] BlockTaskRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new BlockTaskCommand(
                id,
                request.Reason,
                request.ChangedByUserId),
            cancellationToken);

        return NoContent();
    }

    [HttpPut("{id:guid}/complete")]
    public async Task<IActionResult> Complete(
        Guid id,
        [FromBody] Guid completedByUserId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new CompleteTaskCommand(id, completedByUserId),
            cancellationToken);

        return NoContent();
    }

    [HttpPost("{id:guid}/comments")]
    public async Task<IActionResult> AddComment(
        Guid id,
        [FromBody] AddCommentRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new AddTaskCommentCommand(
                id,
                request.UserId,
                request.Content),
            cancellationToken);

        return NoContent();
    }
}
