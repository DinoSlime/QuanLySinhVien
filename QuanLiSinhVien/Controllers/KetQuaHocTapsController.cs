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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KetQuaHocTap>>> GetKetQuaHocTaps()
        {
            return await _context.KetQuaHocTaps.ToListAsync();
        }

        [HttpGet("BangDiem/{maSV}")]
        public async Task<ActionResult<object>> GetBangDiem(string maSV)
        {
            var diemSV = await _context.KetQuaHocTaps.Where(k => k.MaSV == maSV).ToListAsync();
            if (!diemSV.Any()) return NotFound("Chưa có dữ liệu điểm.");
            var gpa = diemSV.Average(k => k.DiemTongKet) ?? 0;
            return Ok(new { MaSV = maSV, GPA = Math.Round(gpa, 2), ChiTietDiem = diemSV });
        }

        [HttpGet("ThongKe/{maLHP}")]
        public async Task<ActionResult<object>> GetThongKeLop(string maLHP)
        {
            var danhSach = await _context.KetQuaHocTaps.Where(k => k.MaLHP == maLHP).ToListAsync();
            if (!danhSach.Any()) return NotFound("Lớp chưa có điểm.");
            int tongSV = danhSach.Count;
            int soSVDat = danhSach.Count(k => k.DiemTongKet >= 5);
            return Ok(new { MaLHP = maLHP, TongSo = tongSV, SoLuongDat = soSVDat, PhanTramDat = Math.Round((double)soSVDat / tongSV * 100, 2) + "%" });
        }

        [HttpGet("{maSV}/{maLHP}")]
        public async Task<ActionResult<KetQuaHocTap>> GetKetQuaHocTap(string maSV, string maLHP)
        {
            var ketQuaHocTap = await _context.KetQuaHocTaps.FindAsync(maSV, maLHP);
            if (ketQuaHocTap == null) return NotFound();
            return ketQuaHocTap;
        }

        [HttpPost("DangKy")]
        public async Task<IActionResult> DangKyHocPhan(string maSV, string maLHP)
        {
            // 1. Kiểm tra tồn tại lớp học phần
            var lop = await _context.LopHocPhans.FindAsync(maLHP);
            if (lop == null) return NotFound("Lớp học phần không tồn tại.");

            // 2. Quy tắc: Chỉ cho phép đăng ký trong thời gian mở
            DateTime hienTai = DateTime.Now;
            if (hienTai < lop.NgayBatDauDangKy || hienTai > lop.NgayKetThucDangKy)
            {
                return BadRequest("Hệ thống đang đóng cổng đăng ký lớp học phần này.");
            }

            // 3. Quy tắc: Ràng buộc sĩ số
            var siSoHienTai = await _context.KetQuaHocTaps.CountAsync(k => k.MaLHP == maLHP);
            if (siSoHienTai >= lop.SiSoToiDa)
            {
                return BadRequest("Lớp đã đầy sĩ số tối đa. Chức năng đăng ký bị khóa.");
            }

            // 4. Quy tắc: Ràng buộc trùng lịch (Kiểm tra Thứ và Tiết bắt đầu)
            var trungLich = await (from kq in _context.KetQuaHocTaps
                                   join l in _context.LopHocPhans on kq.MaLHP equals l.MaLHP
                                   where kq.MaSV == maSV
                                         && l.HocKy == lop.HocKy && l.NamHoc == lop.NamHoc
                                         && l.Thu == lop.Thu && l.TietBatDau == lop.TietBatDau
                                   select l).AnyAsync();

            if (trungLich)
            {
                return BadRequest("Không thể đăng ký do trùng lịch học hiện tại của bạn.");
            }

            // --- 4.5. QUY TẮC MỚI: RÀNG BUỘC MÔN TIÊN QUYẾT ---
            // Lấy danh sách các môn tiên quyết mà môn học này yêu cầu
            var danhSachMonTQ = await _context.MonTienQuyets
                                              .Where(m => m.MaMH == lop.MaMH)
                                              .ToListAsync();

            if (danhSachMonTQ.Any())
            {
                foreach (var tq in danhSachMonTQ)
                {
                    // Tìm điểm tổng kết cao nhất của môn tiên quyết này mà sinh viên đã học
                    var diemTienQuyet = await (from kq in _context.KetQuaHocTaps
                                               join l in _context.LopHocPhans on kq.MaLHP equals l.MaLHP
                                               where kq.MaSV == maSV && l.MaMH == tq.MaMHTQ
                                               orderby kq.DiemTongKet descending
                                               select kq.DiemTongKet).FirstOrDefaultAsync();

                    // Nếu chưa học (null) hoặc điểm dưới 4.0 -> Từ chối đăng ký
                    if (diemTienQuyet == null || diemTienQuyet < 4.0m)
                    {
                        return BadRequest($"Không thể đăng ký. Bạn phải hoàn thành môn tiên quyết ({tq.MaMHTQ}) với điểm từ 4.0 trở lên.");
                    }
                }
            }
            // --------------------------------------------------

            // 5. Lưu dữ liệu (Tiêu chí nghiệm thu thành công)
            var dangKyMoi = new KetQuaHocTap
            {
                MaSV = maSV,
                MaLHP = maLHP,
                DiemQuaTrinh = 0,
                DiemGiuaKy = 0,
                DiemCuoiKy = 0,
                DiemTongKet = 0,
                GhiChu = "Đăng ký thành công"
            };

            try
            {
                _context.KetQuaHocTaps.Add(dangKyMoi);
                await _context.SaveChangesAsync();
                return Ok("Đăng ký học phần thành công.");
            }
            catch (DbUpdateException)
            {
                return Conflict("Sinh viên này đã đăng ký lớp học này rồi.");
            }
        }

        [HttpPut("{maSV}/{maLHP}")]
        public async Task<IActionResult> PutKetQuaHocTap(string maSV, string maLHP, KetQuaHocTap ketQuaHocTap)
        {
            if (maSV != ketQuaHocTap.MaSV || maLHP != ketQuaHocTap.MaLHP) return BadRequest("Mã không khớp.");
            ketQuaHocTap.DiemTongKet = TinhDiem(ketQuaHocTap.DiemQuaTrinh, ketQuaHocTap.DiemGiuaKy, ketQuaHocTap.DiemCuoiKy);
            _context.Entry(ketQuaHocTap).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!KetQuaHocTapExists(maSV, maLHP)) return NotFound(); else throw; }
            return NoContent();
        }

        [HttpDelete("{maSV}/{maLHP}")]
        public async Task<IActionResult> DeleteKetQuaHocTap(string maSV, string maLHP)
        {
            var ketQuaHocTap = await _context.KetQuaHocTaps.FindAsync(maSV, maLHP);
            if (ketQuaHocTap == null) return NotFound();
            _context.KetQuaHocTaps.Remove(ketQuaHocTap);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool KetQuaHocTapExists(string maSV, string maLHP)
        {
            return _context.KetQuaHocTaps.Any(e => e.MaSV == maSV && e.MaLHP == maLHP);
        }

        private decimal? TinhDiem(decimal? qt, decimal? gk, decimal? ck)
        {
            if (qt == null || gk == null || ck == null) return null;
            return (qt * 0.2m) + (gk * 0.3m) + (ck * 0.5m);
        }
    }
}