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
    public class SinhViensController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SinhViensController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. GET: api/SinhViens (Lấy toàn bộ danh sách)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SinhVien>>> GetSinhViens()
        {
            return await _context.SinhViens.ToListAsync();
        }

        // 2. GET: api/SinhViens/Search?keyword=An
        // API Nghiệp vụ: Tìm kiếm nhanh theo Tên hoặc Mã số sinh viên
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<SinhVien>>> SearchSinhVien(string? keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return await _context.SinhViens.ToListAsync();
            }

            var result = await _context.SinhViens
                .Where(s => s.HoTen.Contains(keyword) || s.MaSV.Contains(keyword))
                .ToListAsync();

            return Ok(result);
        }

        // 3. GET: api/SinhViens/Lop/LHP01
        // API Nghiệp vụ: Lấy danh sách sinh viên đang học một lớp cụ thể (Dùng điểm danh)
        [HttpGet("Lop/{maLHP}")]
        public async Task<ActionResult<IEnumerable<object>>> GetSinhViensByLop(string maLHP)
        {
            var result = await (from sv in _context.SinhViens
                                join kq in _context.KetQuaHocTaps on sv.MaSV equals kq.MaSV
                                where kq.MaLHP == maLHP
                                select new
                                {
                                    sv.MaSV,
                                    sv.HoTen,
                                    sv.NgaySinh,
                                    sv.GioiTinh,
                                    sv.TrangThai,
                                    GhiChuDiem = kq.GhiChu
                                }).ToListAsync();

            if (!result.Any())
            {
                return NotFound($"Không tìm thấy sinh viên nào thuộc lớp học phần {maLHP}");
            }

            return Ok(result);
        }

        // 4. GET: api/SinhViens/SV01 (Lấy chi tiết 1 sinh viên)
        [HttpGet("{id}")]
        public async Task<ActionResult<SinhVien>> GetSinhVien(string id)
        {
            var sinhVien = await _context.SinhViens.FindAsync(id);

            if (sinhVien == null)
            {
                return NotFound();
            }

            return sinhVien;
        }

        // 5. PUT: api/SinhViens/SV01 (Cập nhật thông tin)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSinhVien(string id, SinhVien sinhVien)
        {
            if (id != sinhVien.MaSV)
            {
                return BadRequest();
            }

            _context.Entry(sinhVien).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SinhVienExists(id))
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

        // 6. POST: api/SinhViens (Thêm mới sinh viên)
        [HttpPost]
        public async Task<ActionResult<SinhVien>> PostSinhVien(SinhVien sinhVien)
        {
            _context.SinhViens.Add(sinhVien);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SinhVienExists(sinhVien.MaSV))
                {
                    return Conflict("Mã sinh viên này đã tồn tại.");
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSinhVien", new { id = sinhVien.MaSV }, sinhVien);
        }

        // 7. DELETE: api/SinhViens/SV01 (Xóa sinh viên)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSinhVien(string id)
        {
            var sinhVien = await _context.SinhViens.FindAsync(id);
            if (sinhVien == null)
            {
                return NotFound();
            }

            _context.SinhViens.Remove(sinhVien);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SinhVienExists(string id)
        {
            return _context.SinhViens.Any(e => e.MaSV == id);
        }
    }
}