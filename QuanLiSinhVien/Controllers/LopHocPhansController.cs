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

        // 1. GET: api/LopHocPhans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LopHocPhan>>> GetLopHocPhans()
        {
            return await _context.LopHocPhans.ToListAsync();
        }

        // 2. GET: api/LopHocPhans/GiangVien/GV01
        // API Nghiệp vụ: Lấy danh sách các lớp mà một giảng viên cụ thể đang dạy
        [HttpGet("GiangVien/{maGV}")]
        public async Task<ActionResult<IEnumerable<LopHocPhan>>> GetLopByGiangVien(string maGV)
        {
            var result = await _context.LopHocPhans
                .Where(l => l.MaGV == maGV)
                .OrderByDescending(l => l.NamHoc)
                .ThenByDescending(l => l.HocKy)
                .ToListAsync();

            return Ok(result);
        }

        // 3. GET: api/LopHocPhans/Trong
        // API Nghiệp vụ: Tìm các lớp vẫn còn chỗ (Sĩ số thực tế < Sĩ số tối đa)
        [HttpGet("Trong")]
        public async Task<ActionResult<IEnumerable<object>>> GetLopConTrong()
        {
            // Join với bảng KetQuaHocTap để đếm số lượng sinh viên đã đăng ký vào lớp đó
            var result = await _context.LopHocPhans
                .Select(l => new {
                    l.MaLHP,
                    l.MaMH,
                    l.MaGV,
                    l.PhongHoc,
                    l.Thu,
                    l.TietBatDau,
                    l.SiSoToiDa,
                    // Đếm số dòng trong bảng KetQuaHocTap có mã lớp này
                    SiSoThucTe = _context.KetQuaHocTaps.Count(k => k.MaLHP == l.MaLHP)
                })
                .Where(x => x.SiSoThucTe < x.SiSoToiDa)
                .ToListAsync();

            return Ok(result);
        }

        // 4. GET: api/LopHocPhans/5
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

        // 5. PUT: api/LopHocPhans/5
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

        // 6. POST: api/LopHocPhans
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
                    return Conflict("Mã lớp học phần đã tồn tại.");
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetLopHocPhan", new { id = lopHocPhan.MaLHP }, lopHocPhan);
        }

        // 7. DELETE: api/LopHocPhans/5
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