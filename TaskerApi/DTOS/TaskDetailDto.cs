namespace TaskerApi.DTOS
{
    public class TaskDetailDto
    {
        public int OwnerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Status Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}