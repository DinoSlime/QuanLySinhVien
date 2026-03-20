using System.ComponentModel.DataAnnotations;

namespace QuanLySinhVien.Models
{
    public class KetQuaHocTap
    {
        [StringLength(20)]
        public string MaSV { get; set; }

        [StringLength(20)]
        public string MaLHP { get; set; }

        [Range(0, 10)]
        public decimal? DiemQuaTrinh { get; set; }

        [Range(0, 10)]
        public decimal? DiemGiuaKy { get; set; }

        [Range(0, 10)]
        public decimal? DiemCuoiKy { get; set; }

        [Range(0, 10)]
        public decimal? DiemTongKet { get; set; }
    }
}