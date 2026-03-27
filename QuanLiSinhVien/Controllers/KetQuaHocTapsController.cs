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

        // 1. GET: api/KetQuaHocTaps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KetQuaHocTap>>> GetKetQuaHocTaps()
        {
            return await _context.KetQuaHocTaps.ToListAsync();
        }

        // 2. GET: api/KetQuaHocTaps/BangDiem/{maSV}
        // API Nghiệp vụ: Lấy bảng điểm chi tiết và tính GPA tích lũy
        [HttpGet("BangDiem/{maSV}")]
        public async Task<ActionResult<object>> GetBangDiem(string maSV)
        {
            var diemSV = await _context.KetQuaHocTaps.Where(k => k.MaSV == maSV).ToListAsync();

            if (!diemSV.Any())
                return NotFound("Chưa có dữ liệu điểm cho sinh viên này.");

            // Tính GPA (Trung bình cộng của tất cả điểm tổng kết)
            var gpa = diemSV.Average(k => k.DiemTongKet) ?? 0;

            return Ok(new
            {
                MaSinhVien = maSV,
                SoMonDaHoc = diemSV.Count,
                GPA = Math.Round(gpa, 2),
                ChiTietDiem = diemSV
            });
        }

        // 3. GET: api/KetQuaHocTaps/ThongKe/{maLHP}
        // API Nghiệp vụ: Thống kê tỉ lệ Đạt/Trượt của một lớp
        [HttpGet("ThongKe/{maLHP}")]
        public async Task<ActionResult<object>> GetThongKeLop(string maLHP)
        {
            var danhSach = await _context.KetQuaHocTaps.Where(k => k.MaLHP == maLHP).ToListAsync();

            if (!danhSach.Any())
                return NotFound("Lớp học phần này chưa có dữ liệu điểm.");

            int tongSV = danhSach.Count;
            int soSVDat = danhSach.Count(k => k.DiemTongKet >= 5);
            int soSVTruot = tongSV - soSVDat;

            return Ok(new
            {
                MaLopHocPhan = maLHP,
                TongSoSinhVien = tongSV,
                SoLuongDat = soSVDat,
                PhanTramDat = Math.Round((double)soSVDat / tongSV * 100, 2) + "%",
                SoLuongTruot = soSVTruot,
                PhanTramTruot = Math.Round((double)soSVTruot / tongSV * 100, 2) + "%"
            });
        }

        // 4. GET: api/KetQuaHocTaps/{maSV}/{maLHP}
        [HttpGet("{maSV}/{maLHP}")]
        public async Task<ActionResult<KetQuaHocTap>> GetKetQuaHocTap(string maSV, string maLHP)
        {
            var ketQuaHocTap = await _context.KetQuaHocTaps.FindAsync(maSV, maLHP);
            if (ketQuaHocTap == null) return NotFound();
            return ketQuaHocTap;
        }

        // 5. PUT: api/KetQuaHocTaps/{maSV}/{maLHP}
        [HttpPut("{maSV}/{maLHP}")]
        public async Task<IActionResult> PutKetQuaHocTap(string maSV, string maLHP, KetQuaHocTap ketQuaHocTap)
        {
            if (maSV != ketQuaHocTap.MaSV || maLHP != ketQuaHocTap.MaLHP)
                return BadRequest("Mã sinh viên hoặc mã lớp không khớp.");

            // Tự động tính lại điểm tổng kết trước khi cập nhật
            ketQuaHocTap.DiemTongKet = TinhDiem(ketQuaHocTap.DiemQuaTrinh, ketQuaHocTap.DiemGiuaKy, ketQuaHocTap.DiemCuoiKy);

            _context.Entry(ketQuaHocTap).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KetQuaHocTapExists(maSV, maLHP)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // 6. POST: api/KetQuaHocTaps
        [HttpPost]
        public async Task<ActionResult<KetQuaHocTap>> PostKetQuaHocTap(KetQuaHocTap ketQuaHocTap)
        {
            // Tự động tính điểm tổng kết trước khi lưu vào Database
            ketQuaHocTap.DiemTongKet = TinhDiem(ketQuaHocTap.DiemQuaTrinh, ketQuaHocTap.DiemGiuaKy, ketQuaHocTap.DiemCuoiKy);

            _context.KetQuaHocTaps.Add(ketQuaHocTap);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (KetQuaHocTapExists(ketQuaHocTap.MaSV, ketQuaHocTap.MaLHP))
                    return Conflict("Kết quả học tập cho sinh viên này trong lớp này đã tồn tại.");
                else throw;
            }

            return CreatedAtAction("GetKetQuaHocTap", new { maSV = ketQuaHocTap.MaSV, maLHP = ketQuaHocTap.MaLHP }, ketQuaHocTap);
        }

        // 7. DELETE: api/KetQuaHocTaps/{maSV}/{maLHP}
        [HttpDelete("{maSV}/{maLHP}")]
        public async Task<IActionResult> DeleteKetQuaHocTap(string maSV, string maLHP)
        {
            var ketQuaHocTap = await _context.KetQuaHocTaps.FindAsync(maSV, maLHP);
            if (ketQuaHocTap == null) return NotFound();

            _context.KetQuaHocTaps.Remove(ketQuaHocTap);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // --- HÀM HỖ TRỢ (PRIVATE METHODS) ---

        private bool KetQuaHocTapExists(string maSV, string maLHP)
        {
            return _context.KetQuaHocTaps.Any(e => e.MaSV == maSV && e.MaLHP == maLHP);
        }

        // Logic tính điểm: QT*0.2 + GK*0.3 + CK*0.5
        private decimal? TinhDiem(decimal? qt, decimal? gk, decimal? ck)
        {
            if (qt == null || gk == null || ck == null) return null;
            return (qt * 0.2m) + (gk * 0.3m) + (ck * 0.5m);
        }
    }
}