using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Thêm thư viện này để dùng Khóa ngoại

namespace QuanLySinhVien.Models
{
    public class LopHocPhan
    {
        [Key]
        [StringLength(20)]
        public string MaLHP { get; set; }

        [Required]
        [StringLength(20)]
        public string MaMH { get; set; }

        // Khai báo Khóa ngoại liên kết với bảng MonHoc
        [ForeignKey("MaMH")]
        public virtual MonHoc? MonHoc { get; set; }

        [Required]
        [StringLength(20)]
        public string MaGV { get; set; }

        // Khai báo Khóa ngoại liên kết với bảng GiangVien
        [ForeignKey("MaGV")]
        public virtual GiangVien? GiangVien { get; set; }

        [Range(1, 3, ErrorMessage = "Học kỳ phải từ 1 đến 3")]
        public int HocKy { get; set; }

        public int NamHoc { get; set; }

        [Range(1, 200, ErrorMessage = "Sĩ số phải từ 1 đến 200")]
        public int SiSoToiDa { get; set; }

        [StringLength(50)]
        public string? PhongHoc { get; set; }

        [StringLength(20)]
        public string? Thu { get; set; }

        [Range(1, 15)]
        public int? TietBatDau { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu đăng ký là bắt buộc")]
        public DateTime NgayBatDauDangKy { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc đăng ký là bắt buộc")]
        public DateTime NgayKetThucDangKy { get; set; }
    }
}