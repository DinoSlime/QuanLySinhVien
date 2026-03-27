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
    public class GiangViensController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GiangViensController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/GiangViens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GiangVien>>> GetGiangViens()
        {
            return await _context.GiangViens.ToListAsync();
        }

        // GET: api/GiangViens/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GiangVien>> GetGiangVien(string id)
        {
            var giangVien = await _context.GiangViens.FindAsync(id);

            if (giangVien == null)
            {
                return NotFound();
            }

            return giangVien;
        }

        // PUT: api/GiangViens/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGiangVien(string id, GiangVien giangVien)
        {
            if (id != giangVien.MaGV)
            {
                return BadRequest();
            }

            _context.Entry(giangVien).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GiangVienExists(id))
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

        // POST: api/GiangViens
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GiangVien>> PostGiangVien(GiangVien giangVien)
        {
            _context.GiangViens.Add(giangVien);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (GiangVienExists(giangVien.MaGV))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetGiangVien", new { id = giangVien.MaGV }, giangVien);
        }

        // DELETE: api/GiangViens/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGiangVien(string id)
        {
            var giangVien = await _context.GiangViens.FindAsync(id);
            if (giangVien == null)
            {
                return NotFound();
            }

            _context.GiangViens.Remove(giangVien);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GiangVienExists(string id)
        {
            return _context.GiangViens.Any(e => e.MaGV == id);
        }
    }
}
