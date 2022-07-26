using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Entities.CauHinh
{
    public class cd_phien_lam_viec : BaseDanhMucModel
    {
        //[Key]
        public int id { get; set; }
        public string? don_vi { get; set; }
        public int? so_ban { get; set; }
        public int? so_phien { get; set; }
        public int? phien_hien_tai { get; set; }

        [NotMapped]
        public bool isShow { get; set; }
    }
}
