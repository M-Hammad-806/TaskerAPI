using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskerApi.DTOS;
using TaskerApi.Models;
using TaskerApi.Services;

namespace TaskerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController:ControllerBase
    {
        private readonly ITaskService service;
        private readonly ILogger<TaskController> logger;
        public TaskController(ITaskService service, ILogger<TaskController> logger)
        {
            this.service = service;
            this.logger = logger;
        }
        [HttpGet("get-tasks")]
        [Authorize]
        public async Task<ActionResult> GetTasks()
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            logger.GetTasksLog(userId,DateTime.UtcNow);
           // logger.LogInformation("Get Tasks Requested by User {userId} at {date}.", userId, DateTime.Now);
            var res = await service.GetTasks(userId);
            return Ok(res.Result);
        }
        [HttpGet("get-a-task/{id}")]
        [Authorize]
        public async Task<ActionResult> GetATaskById(int id)
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            logger.GetTaskByIdLog(userId, DateTime.UtcNow);

            var res = await service.GetATaskById(userId, id);
            return res.IsSuccess ? Ok(res.Result) : BadRequest(new {res.Title,res.Message});
        }
        [HttpPost("create-a-task")]
        [Authorize]
        public async Task<ActionResult> CreateATask(CreateTaskDto request)
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            logger.CreateATaskLog(userId, DateTime.Now);
            var res = await service.CreateATask(userId,request);
            return res.IsSuccess ? Ok(res.Result) : BadRequest(new { res.Title, res.Message });

        }
        [HttpPut("update-a-task")]
        [Authorize]
        public async Task<ActionResult> UpdateATask(UpdateTaskDto request)
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            logger.UpdateATaskLog(userId, DateTime.Now, request.TaskId);
            var res = await service.UpdateATask(userId, request);
            return res.IsSuccess ? Ok(res.Result) : BadRequest(new { res.Title, res.Message });
        }
        [HttpDelete("delete-a-task/{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteATask(int id)
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            logger.DeleteATaskLog(userId, DateTime.Now, id);
            var res = await service.DeleteATask(userId, id);
            return NoContent();
        }

        //admin-endpoints
        [HttpGet("get-all-tasks")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> GetAllTasks()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var res = await service.GetAllTasks(role);
            return res.IsSuccess ? Ok(res.Result) : BadRequest();
        }
    }
}
