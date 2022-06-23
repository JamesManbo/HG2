using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Entities.Nhom
{
    [Table("Asp_nhom")]
    public class Asp_nhom : BaseModel
    {
        public string ma_nhom { get; set; }
        public string? ten_nhom { get; set; }
        public string? mo_ta { get; set; }
    }

    public class Asp_nhom_paging : BaseModel
    {
        public List<Asp_nhom>? asp_Nhoms { get; set; }
        public Pagelist Pagelist { get; set; } = new Pagelist();
    }
}
