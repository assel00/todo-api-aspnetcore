using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Services
{
    public class TodoService : ITodoService
    {
        private readonly AppDbContext _context;
        public TodoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task Add(TodoItem task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PageResult<TodoDto>> GetAll(bool? isCompleted, string? search, int page, int pageSize, string? sortBy)
        {
            var query = _context.Tasks.AsQueryable();
            if (isCompleted.HasValue)
            {
                query = query.Where(t => t.IsCompleted == isCompleted.Value);
            }
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(t => t.Title.Contains(search));
            }
            var totalCount = await query.CountAsync();

            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "title":
                        query = query.OrderBy(t => t.Title);
                        break;
                    case "title_desc":
                        query = query.OrderByDescending(t => t.Title);
                        break;
                    case "status":
                        query = query.OrderBy(t => t.IsCompleted);
                        break;
                }
            }
            //throw new Exception("Test error");
            var data = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TodoDto
                {
                    Id = t.Id,
                    Title = t.Title
                }).ToListAsync();

            return new PageResult<TodoDto>
            {
                Data = data,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };

        }

        public async Task<TodoItem> GetById(int id)
        {
            return await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task Update(int id, UpdateTodoDto dto)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if (task != null)
            {
                task.Title = dto.Title;
                task.IsCompleted = dto.IsCompleted;
                await _context.SaveChangesAsync();
            }
        }
    }
}
