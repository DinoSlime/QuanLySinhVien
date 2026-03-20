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

        [Range(1, 10, ErrorMessage = "Số tín chỉ phải lớn hơn 0")]
        public int SoTinChi { get; set; }
    }
}