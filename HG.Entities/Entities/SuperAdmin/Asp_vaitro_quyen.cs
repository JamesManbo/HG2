using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Entities.SuperAdmin
{
    [Table("Asp_vaitro_quyen")]
    public class Asp_vaitro_quyen
    {
		public Guid Id { get; set; }
		public string ma_vai_tro { get; set; }
		public string ma_chuc_nang { get; set; }
		public string ds_ma_quyen { get; set; }
		public DateTime? CreatedDateUtc { get; set; }
		public Guid? CreatedUid { get; set; }
		public DateTime? UpdatedDateUtc { get; set; }
		public Guid? UpdatedUid { get; set; }
		public int? Deleted { get; set; }
		public Guid? DeletedBy { get; set; }
        [NotMapped]		
		public string? url { get; set; }
		[NotMapped]
		public string? ten_chuc_nang { get; set; }
		[NotMapped]
		public string? ma_trang_cha { get; set; }

	}

}
