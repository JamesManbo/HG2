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
    [Table("dm_buoc_xu_ly")]
    public class Dm_Buoc_Xu_Ly : BaseDanhMucModel
    {
        [Key]
        public string ma_buoc_xu_ly { get; set; }
        public string ten_buoc_xu_ly { get; set; }
        public string? mo_ta { get; set; }
    }

    public class Dm_Buoc_Xu_Ly_paging
    {
        public List<Dm_Buoc_Xu_Ly>? lstBuocXuLy { get; set; }
        public Pagelist Pagelist { get; set; } = new Pagelist();
    }
}
