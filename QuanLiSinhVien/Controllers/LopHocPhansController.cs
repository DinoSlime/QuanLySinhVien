using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Data;
using QuanLySinhVien.Models;

namespace QuanLiSinhVien.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LopHocPhansController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LopHocPhansController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/LopHocPhans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LopHocPhan>>> GetLopHocPhans()
        {
            return await _context.LopHocPhans.ToListAsync();
        }

        // GET: api/LopHocPhans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LopHocPhan>> GetLopHocPhan(string id)
        {
            var lopHocPhan = await _context.LopHocPhans.FindAsync(id);

            if (lopHocPhan == null)
            {
                return NotFound();
            }

            return lopHocPhan;
        }

        // PUT: api/LopHocPhans/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLopHocPhan(string id, LopHocPhan lopHocPhan)
        {
            if (id != lopHocPhan.MaLHP)
            {
                return BadRequest();
            }

            _context.Entry(lopHocPhan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LopHocPhanExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/LopHocPhans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LopHocPhan>> PostLopHocPhan(LopHocPhan lopHocPhan)
        {
            _context.LopHocPhans.Add(lopHocPhan);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (LopHocPhanExists(lopHocPhan.MaLHP))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetLopHocPhan", new { id = lopHocPhan.MaLHP }, lopHocPhan);
        }

        // DELETE: api/LopHocPhans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLopHocPhan(string id)
        {
            var lopHocPhan = await _context.LopHocPhans.FindAsync(id);
            if (lopHocPhan == null)
            {
                return NotFound();
            }

            _context.LopHocPhans.Remove(lopHocPhan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LopHocPhanExists(string id)
        {
            return _context.LopHocPhans.Any(e => e.MaLHP == id);
        }
    }
}
