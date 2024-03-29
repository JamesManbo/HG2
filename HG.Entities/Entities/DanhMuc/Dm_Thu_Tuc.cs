﻿using HG.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities
{
    public class Dm_Thu_Tuc
    {

        public int ma_thu_tuc_old { get; set; }
        public string ma_thu_tuc { get; set; }
        public string ten_thu_tuc { get; set; }
        public int Deleted { get; set; }
        public bool thuc_hien_hai_gd { get; set; }

    }

    public class Dm_Thanh_Phan_Key
    {

        public int ma_thu_tuc_key { get; set; }
        public string ma_thu_tuc { get; set; }
        public string ten_thanh_phan { get; set; }
        public string ma_thanh_phan { get; set; }
        public int Deleted { get; set; }

    }
}
