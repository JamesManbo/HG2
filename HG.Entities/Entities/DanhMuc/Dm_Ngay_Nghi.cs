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
    [Table("dm_ngay_nghi")]
    public class Dm_Ngay_Nghi : BaseDanhMucModel
    {
        [Key]      
        public int nam { get; set; }
        public string ten_ngay_nghi { get; set; }
        public string? mo_ta { get; set; }
        public string? ngay_nghi_trong_tuan { get; set; }
        public string? thang1 { get; set; }
        public string? thang2 { get; set; }
        public string? thang3 { get; set; }
        public string? thang4 { get; set; }
        public string? thang5 { get; set; }
        public string? thang6 { get; set; }
        public string? thang7 { get; set; }
        public string? thang8 { get; set; }
        public string? thang9 { get; set; }
        public string? thang10 { get; set; }
        public string? thang11 { get; set; }
        public string? thang12 { get; set; }

    }
}
