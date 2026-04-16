using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoApi.DTOs;
using TodoApi.Models;
public interface ITodoService
{
    Task<PageResult<TodoDto>> GetAll(bool? iisCompleted, string? search, int page, int pageSize, string? sortBy);
    Task<TodoItem> GetById(int id);
    Task Add(TodoItem task);
    Task Update(int id, UpdateTodoDto dto);
    Task Delete(int id);
}