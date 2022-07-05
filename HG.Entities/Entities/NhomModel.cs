using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Entities
{
    public class NhomModel : BaseDanhMucModel
    {
        public Guid Id { get; set; }
        public string ma_nhom { get; set; }
        public string? ten_nhom { get; set; }
        public string? mo_ta { get; set; }
        public int? stt { get; set; }
    }
}
