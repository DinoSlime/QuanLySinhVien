using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Models;

namespace QuanLySinhVien.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Đăng ký 6 bảng vào Database
        public DbSet<SinhVien> SinhViens { get; set; }
        public DbSet<GiangVien> GiangViens { get; set; }
        public DbSet<MonHoc> MonHocs { get; set; }
        public DbSet<LopHocPhan> LopHocPhans { get; set; }
        public DbSet<KetQuaHocTap> KetQuaHocTaps { get; set; }
        public DbSet<TaiKhoan> TaiKhoans { get; set; }

        // Thiết lập khóa chính kép cho bảng Điểm (Kết quả học tập)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Thiết lập khóa chính kép cho bảng điểm
            modelBuilder.Entity<KetQuaHocTap>()
                .HasKey(k => new { k.MaSV, k.MaLHP });

            // Cấu hình định dạng điểm số (Lấy 2 số thập phân, tổng 4 chữ số - Ví dụ: 10.00)
            modelBuilder.Entity<KetQuaHocTap>().Property(p => p.DiemQuaTrinh).HasColumnType("decimal(4,2)");
            modelBuilder.Entity<KetQuaHocTap>().Property(p => p.DiemGiuaKy).HasColumnType("decimal(4,2)");
            modelBuilder.Entity<KetQuaHocTap>().Property(p => p.DiemCuoiKy).HasColumnType("decimal(4,2)");
            modelBuilder.Entity<KetQuaHocTap>().Property(p => p.DiemTongKet).HasColumnType("decimal(4,2)");

            base.OnModelCreating(modelBuilder);
        }
    }
}