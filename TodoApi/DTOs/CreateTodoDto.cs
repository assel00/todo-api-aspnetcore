using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTOs
{
    public class CreateTodoDto
    {
        [Required]
        public string Title { get; set; }
    }
}
