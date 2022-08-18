using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities
{
    [Table("Asp_user_client")]
    public class Asp_user_client
    {
        [Key]
        public int Id { get; set; }
        public Guid? ma_nguoi_dung { get; set; }
        public string? loai_giay_to { get; set; }
        public string? so_giay_to { get; set; }
        public string? ten_co_quan { get; set; }
        public string? dia_chi_co_quan { get; set; }
        public string? dia_chi { get; set; }
        public string? ma_tinh_thanh { get; set; }
        public string? ma_quan_huyen { get; set; }
        public string? ma_xa_phuong { get; set; }
        public string? code_verify { get; set; }
    }
}
