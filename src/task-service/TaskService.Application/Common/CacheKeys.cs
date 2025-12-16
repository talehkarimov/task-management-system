namespace TaskService.Application.Common;

public static class CacheKeys
{
    public static string TaskById(Guid taskId)
        => $"task:{taskId}";

    public static string TasksByProject(Guid projectId, int page, int pageSize)
        => $"tasks:project:{projectId}:{page}:{pageSize}";

    public static string SearchTasks(string term,Guid? projectId,int page,int pageSize)
       => $"tasks:search:{term}:{projectId}:{page}:{pageSize}";
}
