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
    [Table("luong_xu_ly")]
    public class Dm_Luong_Xu_Ly : BaseDanhMucModel
    {
        [Key]
        public string ma_luong { get; set; }
        public string ten_luong { get; set; }
        public string? mo_ta { get; set; }
        public bool tt_hai_gd { get; set; }
        [NotMapped]
        public int tong_ngay_tt { get; set; }
        [NotMapped]
        public float tong_ngay_qt { get; set; }
        public string? file_excel { get; set; }
        public string ma_thu_tuc { get; set; }
        public string? ma_thu_tuc_map { get; set; }
        public string? luong_cha { get; set; }

        [NotMapped]
        public string? lst_thu_tuc { get; set; }

        [NotMapped]
        public bool buoc_xl_chinh { get; set; }
    }

    public class Dm_Luong_Xu_Ly_paging
    {
        public List<Dm_Luong_Xu_Ly>? lstLuongXuLy { get; set; }
        public Pagelist Pagelist { get; set; } = new Pagelist();
    }

    public class Dm_luong_Key
    {
        public string ma_luong { get; set; }
        public string ten_luong { get; set; }

    }
}
