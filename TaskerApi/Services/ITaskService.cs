using TaskerApi.DTOS;

namespace TaskerApi.Services
{
    public interface ITaskService
    {
        Task<ServiceResult<TaskDetailDto>> CreateATask(int userId, CreateTaskDto request);
        Task<bool> DeleteATask(int userId, int taskId);
        Task<ServiceResult<TaskDetailDto>> GetATaskById(int userId, int taskId);
        Task<ServiceResult<List<TaskDetailDto>>> GetTasks(int userId);
        Task<ServiceResult<List<TaskDetailDto>>> GetAllTasks(string userId);

        Task<ServiceResult<TaskDetailDto>> UpdateATask(int userId, UpdateTaskDto request);
    }
}