﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Entities.News
{
    public class TinTuc : BaseEntities
    {
        public int? id_dm_kenh_tin {get;set;}
        public string? tieu_de { get; set; }
        public string? noi_dung { get; set; }
        public string? noi_dung_chi_tiet { get; set; }
        public string? anh_dai_dien { get; set; }
        public bool? is_hien_thi {get;set;}
        public int? status { get; set; }
        public int? stt { get; set; }
    }
}