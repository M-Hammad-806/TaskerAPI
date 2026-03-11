using System.ComponentModel.DataAnnotations;

namespace TaskerApi.DTOS
{

    public class UpdateTaskDto
    {
        public int TaskId { get; set; }
        [Required][StringLength(maximumLength: 200)] public string Name { get; set; } = string.Empty;
        [Required] public string Description { get; set; } = string.Empty;
        [Required] public Status Status { get; set; } = Status.Todo;

    }
}