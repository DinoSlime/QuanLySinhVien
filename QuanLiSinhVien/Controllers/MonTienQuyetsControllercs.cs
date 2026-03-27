using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Data;
using QuanLySinhVien.Models;

namespace QuanLiSinhVien.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonTienQuyetsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MonTienQuyetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Xem danh sách các môn tiên quyết của 1 môn học
        [HttpGet("{maMH}")]
        public async Task<ActionResult<IEnumerable<MonTienQuyet>>> GetMonTienQuyet(string maMH)
        {
            return await _context.MonTienQuyets.Where(m => m.MaMH == maMH).ToListAsync();
        }

        // Thêm một điều kiện môn tiên quyết mới
        [HttpPost]
        public async Task<ActionResult<MonTienQuyet>> PostMonTienQuyet(MonTienQuyet monTienQuyet)
        {
            _context.MonTienQuyets.Add(monTienQuyet);
            try
            {
                await _context.SaveChangesAsync();
                return Ok("Thêm môn tiên quyết thành công!");
            }
            catch (DbUpdateException)
            {
                return Conflict("Điều kiện tiên quyết này đã tồn tại.");
            }
        }
    }
}