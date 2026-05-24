using InfinityDesk.Api.Data;
using InfinityDesk.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InfinityDesk.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext context;

        public TasksController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();

            var tasks = await this.context.Tasks
                .Where(t => t.UserId == userId)
                .ToListAsync();

            return Ok(tasks);
        }

      
        // GET: api/tasks/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = GetUserId();

            var task = await this.context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

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

            task.UserId = GetUserId(); //ignorisemo id koji korisnik posalje u bodyju, uvek se uzima iz tokena
            this.context.Tasks.Add(task);
            await this.context.SaveChangesAsync();
            return Ok(task);
        }

        // PUT: api/tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TaskItem updatedTask)
        {
            var userId = GetUserId();
            var task = await this.context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

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
            var userId = GetUserId();

            var task = await this.context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null)
            {
                return NotFound();
            }

            this.context.Tasks.Remove(task);

            await this.context.SaveChangesAsync();

            return NoContent();
        }

        // User je property koji dolazi iz ControllerBase klase
        // Sadrzi podatke o ulogovanom korisniku koje smo stavili u token kao Claims
        // FindFirstValue trazi Claim po tipu i vraca njegovu vrednost kao string
        // ClaimTypes.NameIdentifier je Id korisnika koji smo stavili u token u TokenService
        // int.Parse pretvara string u broj jer Claims uvek cuva stringove
        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}