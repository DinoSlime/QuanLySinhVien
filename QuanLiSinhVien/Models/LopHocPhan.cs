using System.ComponentModel.DataAnnotations;

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

        [Required]
        [StringLength(20)]
        public string MaGV { get; set; }

        [Range(1, 3)]
        public int HocKy { get; set; }

        public int NamHoc { get; set; }

        [Range(1, int.MaxValue)]
        public int SiSoToiDa { get; set; }
    }
}