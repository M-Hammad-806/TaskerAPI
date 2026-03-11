using TaskerApi.Controllers;
using TaskerApi.Services;
namespace TaskerApi
{
    public static partial class Log
    {
        [LoggerMessage(20, LogLevel.Information, "Get Tasks Requested by User {userId} at {date}.")]
        public static partial void GetTasksLog(
            this ILogger<TaskController> logger,
            int userId,
            DateTime date);

        [LoggerMessage(21, LogLevel.Information, "Get Task By Id Requested by User {userId} at {date}.")]
        public static partial void GetTaskByIdLog(
         this ILogger<TaskController> logger,
         int userId,
         DateTime date);

        [LoggerMessage(22, LogLevel.Information, "Create A Task Requested by {userId} at {date}.")]
        public static partial void CreateATaskLog(
      this ILogger<TaskController> logger,
      int userId,
      DateTime date);
        [LoggerMessage(23, LogLevel.Information, "Update A Task Requested by {userId} at {date} Task Id : {taskId}.")]
        public static partial void UpdateATaskLog(
         this ILogger<TaskController> logger,
         int userId,
         DateTime date, int taskId);


        [LoggerMessage(24, LogLevel.Information, "Delete A Task Requested by {userId} at {date} Task Id : {taskId}.")]
        public static partial void DeleteATaskLog(
         this ILogger<TaskController> logger,
         int userId,
         DateTime date, int taskId);

        //auth log methods
        [LoggerMessage(30, LogLevel.Information, "Refresh Token Action Triggered at date : {date}.")]
        public static partial void RefreshTokenRequestLog(
       this ILogger<UserAuthController> logger,
       DateTime date);

        [LoggerMessage(31, LogLevel.Error, "Invalid Refresh Token Given  at date : {date}.")]
        public static partial void InvalidRefreshTokenLog(
      this ILogger<UserAuthService> logger,

      DateTime date);
    }
}
