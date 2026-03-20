using System;
using System.ComponentModel.DataAnnotations;

namespace QuanLySinhVien.Models
{
    public class SinhVien
    {
        [Key]
        [StringLength(20)]
        public string MaSV { get; set; }

        [Required]
        [StringLength(100)]
        public string HoTen { get; set; }

        public DateTime? NgaySinh { get; set; }

        [StringLength(10)]
        public string GioiTinh { get; set; }

        [StringLength(200)]
        public string QueQuan { get; set; }

        [StringLength(50)]
        public string MaNganh { get; set; }
    }
}