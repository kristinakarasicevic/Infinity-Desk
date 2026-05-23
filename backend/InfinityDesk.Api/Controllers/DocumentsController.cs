using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InfinityDesk.Api.Data;
using InfinityDesk.Api.Models;

namespace InfinityDesk.Api.Controllers
{
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
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var documents = await this.context.Documents
                .Where(d => d.UserId == userId)
                .ToListAsync();

            return Ok(documents);
        }

        // GET: api/documents/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var document = await this.context.Documents.FindAsync(id);

            if (document == null)
            {
                return NotFound();
            }

            return Ok(document);
        }

        // POST: api/documents
        [HttpPost]
        public async Task<IActionResult> Create(Document document)
        {
            this.context.Documents.Add(document);

            await this.context.SaveChangesAsync();

            return Ok(document);
        }

        // PUT: api/documents/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Document updated)
        {
            var document = await this.context.Documents.FindAsync(id);

            if (document == null)
            {
                return NotFound();
            }

            document.FileName = updated.FileName;
            document.StoredFileName = updated.StoredFileName;

            await this.context.SaveChangesAsync();

            return Ok(document);
        }

        // DELETE: api/documents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var document = await this.context.Documents.FindAsync(id);

            if (document == null)
            {
                return NotFound();
            }

            this.context.Documents.Remove(document);

            await this.context.SaveChangesAsync();

            return NoContent();
        }
    }
}