using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InfinityDesk.Api.Data;
using InfinityDesk.Api.Models;

namespace InfinityDesk.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext context;

        public TasksController(AppDbContext context)
        {
            this.context = context;
        }

        // GET: api/tasks/user/1
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var tasks = await this.context.Tasks
                .Where(t => t.UserId == userId)
                .ToListAsync();

            return Ok(tasks);
        }

        // GET: api/tasks/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await this.context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        // POST: api/tasks
        [HttpPost]
        public async Task<IActionResult> Create(TaskItem task)
        {
            this.context.Tasks.Add(task);

            await this.context.SaveChangesAsync();

            return Ok(task);
        }

        // PUT: api/tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TaskItem updatedTask)
        {
            var task = await this.context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            task.Text = updatedTask.Text;
            task.IsCompleted = updatedTask.IsCompleted;

            await this.context.SaveChangesAsync();

            return Ok(task);
        }

        // DELETE: api/tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await this.context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            this.context.Tasks.Remove(task);

            await this.context.SaveChangesAsync();

            return NoContent();
        }
    }
}