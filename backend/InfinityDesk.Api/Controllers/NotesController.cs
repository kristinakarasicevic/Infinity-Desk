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
    public class NotesController : ControllerBase
    {

        private readonly AppDbContext context;

        public NotesController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();
            var notes = await this.context.Notes.Where(
                n => n.UserId == userId).ToListAsync();

            return Ok(notes);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = GetUserId();
            var note = await this.context.Notes
                .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);
            if (note == null)
            {
                return NotFound();
            }
            return Ok(note);
        }


        [HttpPost]
        public async Task<IActionResult> Create(Note note)
        {
            note.UserId = GetUserId();//ignorise se id koji je korisnik poslao u bodyju
            this.context.Notes.Add(note);
            await context.SaveChangesAsync();
            return Ok(note);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Note updatedNote)
        {
            var userId = GetUserId();
            //korisnik moze da menja samo svoje beleske
            var note = await this.context.Notes
                .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);
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
            var userId = GetUserId();
            //korisnik moze da brise samo svoje beleske
            var note = await this.context.Notes
                .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);
            if (note == null)
            {
                return NotFound();
            }

            this.context.Notes.Remove(note);
            await context.SaveChangesAsync();
            return NoContent();
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}