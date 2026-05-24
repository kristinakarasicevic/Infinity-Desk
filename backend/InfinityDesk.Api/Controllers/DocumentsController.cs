using InfinityDesk.Api.Data;
using InfinityDesk.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InfinityDesk.Api.Controllers
{
    [Authorize] //znaci da svaki endpoint u ovom kontroleru zahteva validan JWT token
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly AppDbContext context;

        public DocumentsController(AppDbContext context)
        {
            this.context = context;
        }

        // GET: api/documents/user/1
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();
            var documents = await this.context.Documents
                .Where(d => d.UserId == userId)
                .ToListAsync();

            return Ok(documents);
        }

        // GET: api/documents/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = GetUserId();
            var document = await this.context.Documents.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (document == null)
            {
                return NotFound();
            }

            return Ok(document);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {

            if (file == null || file.Length == 0)
            {
                return BadRequest("Nije odabran fajl");
            }

            var userId = GetUserId();

            // Pravljenje putanje do foldera wwwroot/uploads
            // GetCurrentDirectory() vraca folder gde se projekat izvrsava
            // Path.Combine spaja delove putanje na ispravan nacin za svaki OS
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            // Kreira folder ako ne postoji, ako postoji ne radi nista
            Directory.CreateDirectory(uploadsFolder);


            // Guid.NewGuid() generise jedinstveni ID
            // da ne bi doslo do konflikta ako dva korisnika uploadaju fajl istog imena!!!!!!
            // Path.GetExtension uzima ekstenziju originalnog fajla npr. S.pdf
            // rezultat je ID.pdf
            var storedFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            // Puna putanja do fajla na disku npr. "C: .../wwwroot/uploads/ID.pdf
            var filePath = Path.Combine(uploadsFolder, storedFileName);

            // Otvara stream ka fajlu na disku i kopira sadrzaj
            // using osigurava da se stream zatvori nakon kopiranja, oslobadja memoriju
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var document = new Document
            {
                FileName = file.FileName, //originalno ime fajla koje je korisnik imao na svom racunaru
                StoredFileName = storedFileName, //ime fajla kako je sacuvan na serveru
                UserId = userId
            };

            this.context.Documents.Add(document);
            await this.context.SaveChangesAsync();

            return Ok(document);
        }


        // DELETE: api/documents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();
            var document = await this.context.Documents.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (document == null)
            {
                return NotFound();
            }

            //brisanje fajla sa diska
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", document.StoredFileName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            this.context.Documents.Remove(document);
            await this.context.SaveChangesAsync();
            return NoContent();
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

    }
}