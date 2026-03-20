using System.ComponentModel.DataAnnotations;

namespace QuanLySinhVien.Models
{
    public class TaiKhoan
    {
        [Key]
        [StringLength(50)]
        public string TenDangNhap { get; set; }

        [Required]
        [StringLength(255)]
        public string MatKhau { get; set; }

        [StringLength(20)]
        public string Quyen { get; set; }

        public bool TrangThai { get; set; } = true;
    }
}