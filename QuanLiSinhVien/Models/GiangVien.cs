using System.ComponentModel.DataAnnotations;

namespace QuanLySinhVien.Models
{
    public class GiangVien
    {
        [Key]
        [StringLength(20)]
        public string MaGV { get; set; }

        [Required]
        [StringLength(100)]
        public string HoTen { get; set; }

        [StringLength(100)]
        public string KhoaBoMon { get; set; }

        [StringLength(50)]
        public string HocHamHocVi { get; set; }
    }
}