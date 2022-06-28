using HG.Entities.Entities.Nhom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Entities
{
    public class VaitroModel : BaseModel
    {
        public List<Asp_dm_vai_tro> asp_Dm_Vai_Tro { get; set; }
        public string danh_sach_nhom_vai_tro { get; set; }
        
    }
}
