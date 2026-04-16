using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TodoApi.DTOs;
using TodoApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;
        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }
        [HttpGet]
        public async Task<PageResult<TodoDto>> GetAllTasks(bool? isCompleted, string? search, int page = 1, int pageSize = 5, string? sortBy = null)
        {
            return await _todoService.GetAll(isCompleted, search, page, pageSize, sortBy);
        }

        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] CreateTodoDto dto)
        {
            var task = new TodoItem
            {
                Title = dto.Title,
                IsCompleted = false
            };
            await _todoService.Add(task);
            return Ok(task);
        }
        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetTaskById(int taskId)
        {
            var task = await _todoService.GetById(taskId);

            if (task == null)
            {
                return NotFound("Task not found");
            }
            return Ok(task);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTodoDto dto)
        {
            var task = _todoService.GetById(id);

            if (task == null)
            {
                return NotFound("Task not found");
            }

            await _todoService.Update(id, dto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _todoService.GetById(id);

            if (task == null)
            {
                return NotFound("Task not found");
            }

            await _todoService.Delete(id);

            return NoContent();

        }
    }
}
