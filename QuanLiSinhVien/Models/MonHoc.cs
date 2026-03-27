using System;
using System.ComponentModel.DataAnnotations;

namespace QuanLySinhVien.Models
{
    public class MonHoc
    {
        [Key]
        [StringLength(20)]
        public string MaMH { get; set; }

        [Required(ErrorMessage = "Tên môn học không được để trống")]
        [StringLength(100)]

        public string TenMon { get; set; }

        [Range(1, 10, ErrorMessage = "Số tín chỉ phải từ 1 đến 10")]
        public int SoTinChi { get; set; }


        [StringLength(50)]
        public string? LoaiMon { get; set; } 

        [Range(0, 150)]
        public int? SoTietLyThuyet { get; set; }

        [Range(0, 150)]
        public int? SoTietThucHanh { get; set; }
    }
}