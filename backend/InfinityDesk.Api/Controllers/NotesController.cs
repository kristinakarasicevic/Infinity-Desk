using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InfinityDesk.Api.Data;
using InfinityDesk.Api.Models;

namespace InfinityDesk.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {

        private readonly AppDbContext context;

        public NotesController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var notes = await this.context.Notes.Where(
                n => n.UserId == userId).ToListAsync();

            return Ok(notes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var note = await this.context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            return Ok(note);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Note note)
        {
            this.context.Notes.Add(note);
            await context.SaveChangesAsync();
            return Ok(note);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Note updatedNote)
        {
            var note = await this.context.Notes.FindAsync(id);
            if (note == null)
            {

                return NotFound();
            }

            note.Title = updatedNote.Title;
            note.Content = updatedNote.Content;

            await context.SaveChangesAsync();
            return Ok(note);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var note = await this.context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            this.context.Notes.Remove(note);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}