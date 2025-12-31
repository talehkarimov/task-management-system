using Microsoft.EntityFrameworkCore;
using TaskService.Domain.Entitites;

namespace TaskService.Application.Interfaces;

public interface ITaskReadDbContext
{
    DbSet<TaskItem> Tasks { get; }
}
