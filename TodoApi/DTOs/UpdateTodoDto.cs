using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTOs
{
    public class UpdateTodoDto
    {
        [Required]
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }
}
