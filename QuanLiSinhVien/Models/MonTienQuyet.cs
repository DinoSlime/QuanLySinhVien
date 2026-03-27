using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLySinhVien.Models
{
    public class MonTienQuyet
    {
        [Key, Column(Order = 0)]
        [StringLength(20)]
        public string MaMH { get; set; } 

        [Key, Column(Order = 1)]
        [StringLength(20)]
        public string MaMHTQ { get; set; } 
    }
}