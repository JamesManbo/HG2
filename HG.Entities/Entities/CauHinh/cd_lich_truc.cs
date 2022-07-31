using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Entities.CauHinh
{
    public class cd_lich_truc
    {
        [Key]
        public int id { get; set; }
        public string? don_vi { get; set; }
        public int? tuan { get; set; }
        public int? nam { get; set; }
        public int? thang { get; set; }
        public string? thu2 { get; set; }
        public string? thu3 { get; set; }
        public string? thu4 { get; set; }
        public string? thu5 { get; set; }
        public string? thu6 { get; set; }
        public string? thu7 { get; set; }
        public string? thu8 { get; set; }

    }

    public class lich_truc_model
    {
        public cd_lich_truc lich_truc { get; set; }
        public Dm_Ngay_Nghi ngay_nghi { get; set; }
        public List<Dm_Ngay_Nghi> lst_ngay_nghi { get; set; }
    }
}
