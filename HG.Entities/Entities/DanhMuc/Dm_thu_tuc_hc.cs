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
    [Table("Dm_thu_tuc_hc")]
    public class Dm_thu_tuc_hc : BaseDanhMucModel
    {

        
        public string ma_thu_tuc { get; set; }
        public string ten_thu_tuc { get; set; }
       

    }

   
}
