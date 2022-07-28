using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Entities.CauHinh
{
    [Table("cd_danh_sach_can_bo")]
    public class cd_danh_sach_can_bo : BaseDanhMucModel
    {
        [Key]
        public int id { get; set; }
        public Guid? user_id { get; set; }
        public string? ten_can_bo { get; set; }
        public string? don_vi { get; set; }
        [NotMapped]
        public string? ten_don_vi { get; set; }

    }
}
