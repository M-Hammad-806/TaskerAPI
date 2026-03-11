using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskerApi.Data;
using TaskerApi.DTOS;
using TaskerApi.Models;

namespace TaskerApi.Services
{
    public class TaskService(AppDbContext context) : ITaskService
    {
        public async Task<ServiceResult<List<TaskDetailDto>>> GetTasks(int userId)
        {
            var res = await context.Tasks
                .Select(t => new TaskDetailDto
                {
                    Name = t.Name,
                    Description = t.Description,
                    OwnerId = t.OwnerId,
                    Status = t.Status,
                    CreatedAt = t.CreatedAt
                })
                .Where(t => t.OwnerId == userId).ToListAsync();
            return new ServiceResult<List<TaskDetailDto>>().Success(res);
        }

        public async Task<ServiceResult<List<TaskDetailDto>>> GetAllTasks(string role)
        {
          
            if (role != "Admin")
                return new ServiceResult<List<TaskDetailDto>>().Fail("User not Authorized", "User doesn't have credentials to access all tasks");

            var res = await context.Tasks
                .Select(t => new TaskDetailDto
                {
                    Name = t.Name,
                    Description = t.Description,
                    OwnerId = t.OwnerId,
                    Status = t.Status,
                    CreatedAt = t.CreatedAt
                })
                .ToListAsync();
            
            return new ServiceResult<List<TaskDetailDto>>().Success(res);
        }

        public async Task<ServiceResult<TaskDetailDto>> GetATaskById(int userId, int taskId)
        {
            var task = await context.Tasks.Where(t => t.Id == taskId && t.OwnerId == userId).FirstOrDefaultAsync();
            if (task is null)
                return new ServiceResult<TaskDetailDto>().Fail("Not Found", "Task not found");
            return new ServiceResult<TaskDetailDto>()
                .Success(new TaskDetailDto
                {
                    Name = task.Name,
                    Description = task.Description,
                    Status = task.Status,
                    OwnerId = task.OwnerId,
                    CreatedAt = task.CreatedAt
                });


        }

        public async Task<ServiceResult<TaskDetailDto>> CreateATask(int userId, CreateTaskDto request)
        {
            ATask task = new ATask
            {
                Name = request.Name,
                Description = request.Description,
                OwnerId = userId,
                Status = request.Status
            };
            task.CreatedAt = DateTime.UtcNow;
            await context.Tasks.AddAsync(task);
            await context.SaveChangesAsync();
            return new ServiceResult<TaskDetailDto>()
                .Success(new TaskDetailDto
                {
                    Name = task.Name,
                    Description = task.Description,
                    OwnerId = task.OwnerId,
                    Status = task.Status,
                    CreatedAt = task.CreatedAt
                });
        }

        public async Task<ServiceResult<TaskDetailDto>> UpdateATask(int userId, UpdateTaskDto request)
        {
            var task = await context.Tasks
                .Where(t => t.OwnerId == userId && t.Id == request.TaskId)
                .FirstOrDefaultAsync();
            if (task is null)
                return new ServiceResult<TaskDetailDto>().Fail("Task not found", "Task with given id doesn't exists");

            task.Name = request.Name;
            task.Description = request.Description;
            task.Status = request.Status;

            context.Tasks.Update(task);
            await context.SaveChangesAsync();
            
            return new ServiceResult<TaskDetailDto>()
                .Success(new TaskDetailDto
                {
                    Name = task.Name,
                    Description = task.Description,
                    OwnerId = task.OwnerId,
                    Status = task.Status,
                    CreatedAt = task.CreatedAt
                });
        }
        public async Task<bool> DeleteATask(int userId, int taskId)
        {
            var res = await context.Tasks.Where(t => t.OwnerId == userId && t.Id == taskId).ExecuteDeleteAsync();
            return res > 0;
        }
    }
}
