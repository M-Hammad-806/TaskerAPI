namespace TaskerApi.Models
{
    public class ATask
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Status Status { get; set; }
        public int OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
