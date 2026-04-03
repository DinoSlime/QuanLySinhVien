using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using QuanLySinhVien.Data;
using QuanLySinhVien.Models;

namespace QuanLiSinhVien.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaiKhoansController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration; // Thêm biến này để đọc cấu hình JWT

        public TaiKhoansController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // --- HÀM MỚI: ĐĂNG NHẬP VÀ CẤP TOKEN ---
        [HttpPost("DangNhap")]
        public async Task<IActionResult> DangNhap([FromBody] LoginModel model)
        {
            // 1. Tìm tài khoản trong Database (Kèm theo điều kiện TrangThai = true)
            var user = await _context.TaiKhoans
                .FirstOrDefaultAsync(u => u.TenDangNhap == model.TenDangNhap && u.MatKhau == model.MatKhau && u.TrangThai == true);

            if (user == null)
            {
                return Unauthorized("Tên đăng nhập hoặc mật khẩu không đúng, hoặc tài khoản đã bị khóa.");
            }

            // 2. Chế tạo "Thẻ thông hành" (JWT Token)
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.TenDangNhap),
                    new Claim(ClaimTypes.Role, user.Quyen) // Đưa quyền vào Token để sau này phân quyền
                }),
                Expires = DateTime.UtcNow.AddHours(2), // Token có hiệu lực 2 giờ
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // 3. Trả Token về cho client
            return Ok(new
            {
                ThongBao = "Đăng nhập thành công",
                Token = tokenString,
                Quyen = user.Quyen
            });
        }

        // GET: api/TaiKhoans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaiKhoan>>> GetTaiKhoans()
        {
            return await _context.TaiKhoans.ToListAsync();
        }

        // GET: api/TaiKhoans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaiKhoan>> GetTaiKhoan(string id)
        {
            var taiKhoan = await _context.TaiKhoans.FindAsync(id);

            if (taiKhoan == null)
            {
                return NotFound();
            }

            return taiKhoan;
        }

        // PUT: api/TaiKhoans/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaiKhoan(string id, TaiKhoan taiKhoan)
        {
            if (id != taiKhoan.TenDangNhap)
            {
                return BadRequest();
            }

            _context.Entry(taiKhoan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaiKhoanExists(id))
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

        // POST: api/TaiKhoans
        [HttpPost]
        public async Task<ActionResult<TaiKhoan>> PostTaiKhoan(TaiKhoan taiKhoan)
        {
            _context.TaiKhoans.Add(taiKhoan);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TaiKhoanExists(taiKhoan.TenDangNhap))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTaiKhoan", new { id = taiKhoan.TenDangNhap }, taiKhoan);
        }

        // DELETE: api/TaiKhoans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaiKhoan(string id)
        {
            var taiKhoan = await _context.TaiKhoans.FindAsync(id);
            if (taiKhoan == null)
            {
                return NotFound();
            }

            _context.TaiKhoans.Remove(taiKhoan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaiKhoanExists(string id)
        {
            return _context.TaiKhoans.Any(e => e.TenDangNhap == id);
        }
    }

    // Lớp phụ dùng để nhận dữ liệu JSON từ body khi đăng nhập
    public class LoginModel
    {
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
    }
}