using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.HoSo
{
    [Table("ho_so_thanh_phan")]
    public class ho_so_thanh_phan
    {
        [Key]
        public int Id { get; set; }
        public int ho_so_id { get; set; }
        public string ma_thanh_phan { get; set; }
        public string? file_dinh_kem { get; set; }
    }
}
