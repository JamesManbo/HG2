using HG.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities
{
    [Table("ghs_tuyen_sinh_cap_thpt_hoso")]
    public class Ghs_Tuyen_Sinh_Cap_THPT_Hoso : BaseDanhMucModel
    {
        [Key]
        public int Id { get; set; }
        public string? ten_thanh_phan { get; set; }
        public string? file_dinh_kem { get; set; }
        public string? bieu_mau   { get; set; }
        public int? ban_chinh { get; set; }
        public int? ban_sao { get; set; }
        public int? ban_photo { get; set; }
        public int? id_ghs_tuyen_sinh_cap_thpt { get; set; }
        
        public int? Stt { get; set; }
    }
}
