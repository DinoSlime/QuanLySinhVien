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
    public class KetQuaHocTapsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public KetQuaHocTapsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/KetQuaHocTaps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KetQuaHocTap>>> GetKetQuaHocTaps()
        {
            return await _context.KetQuaHocTaps.ToListAsync();
        }

        // GET: api/KetQuaHocTaps/{maSV}/{maLHP}
        [HttpGet("{maSV}/{maLHP}")]
        public async Task<ActionResult<KetQuaHocTap>> GetKetQuaHocTap(string maSV, string maLHP)
        {
            // SỬA: Truyền cả 2 khóa vào FindAsync
            var ketQuaHocTap = await _context.KetQuaHocTaps.FindAsync(maSV, maLHP);

            if (ketQuaHocTap == null)
            {
                return NotFound();
            }

            return ketQuaHocTap;
        }

        // PUT: api/KetQuaHocTaps/{maSV}/{maLHP}
        [HttpPut("{maSV}/{maLHP}")]
        public async Task<IActionResult> PutKetQuaHocTap(string maSV, string maLHP, KetQuaHocTap ketQuaHocTap)
        {
            // SỬA: Kiểm tra khớp cả 2 mã
            if (maSV != ketQuaHocTap.MaSV || maLHP != ketQuaHocTap.MaLHP)
            {
                return BadRequest("Mã sinh viên hoặc mã lớp học phần không khớp với dữ liệu.");
            }

            _context.Entry(ketQuaHocTap).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // SỬA: Hàm kiểm tra tồn tại cũng phải nhận 2 tham số
                if (!KetQuaHocTapExists(maSV, maLHP))
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

        // POST: api/KetQuaHocTaps
        [HttpPost]
        public async Task<ActionResult<KetQuaHocTap>> PostKetQuaHocTap(KetQuaHocTap ketQuaHocTap)
        {
            _context.KetQuaHocTaps.Add(ketQuaHocTap);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // SỬA: Kiểm tra trùng lặp bằng cả 2 mã
                if (KetQuaHocTapExists(ketQuaHocTap.MaSV, ketQuaHocTap.MaLHP))
                {
                    return Conflict("Kết quả học tập này đã tồn tại.");
                }
                else
                {
                    throw;
                }
            }

            // SỬA: Trả về route chứa cả 2 mã sau khi tạo thành công
            return CreatedAtAction("GetKetQuaHocTap", new { maSV = ketQuaHocTap.MaSV, maLHP = ketQuaHocTap.MaLHP }, ketQuaHocTap);
        }

        // DELETE: api/KetQuaHocTaps/{maSV}/{maLHP}
        [HttpDelete("{maSV}/{maLHP}")]
        public async Task<IActionResult> DeleteKetQuaHocTap(string maSV, string maLHP)
        {
            // SỬA: Tìm kiếm bằng cả 2 khóa
            var ketQuaHocTap = await _context.KetQuaHocTaps.FindAsync(maSV, maLHP);
            if (ketQuaHocTap == null)
            {
                return NotFound();
            }

            _context.KetQuaHocTaps.Remove(ketQuaHocTap);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Hàm hỗ trợ kiểm tra tồn tại
        private bool KetQuaHocTapExists(string maSV, string maLHP)
        {
            // SỬA: Check tồn tại bằng cách so khớp cả 2 trường
            return _context.KetQuaHocTaps.Any(e => e.MaSV == maSV && e.MaLHP == maLHP);
        }
    }
}