﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Entities.Model
{
    public class MenuModel : Pagelist
    {
        public string? tu_khoa { get; set; }
        public int? level { get; set; }
    }

    public class DanhMucModel : Pagelist
    {
        public string? tu_khoa { get; set; }
    }

    public class QuyTrinhModel : Pagelist
    {
        public string? tu_khoa { get; set; }
        public string? ma_luong { get; set; }
    }

    public class ThuTucModels : Pagelist
    {
        public string? ma_pb { get; set; }
        public string? ma_lv { get; set; }
        public string? tu_khoa { get; set; }
        public Guid? userId { get; set; }
    }
    public class ThuTucCongDanModels : Pagelist
    {
        public string? donvi { get; set; }
        public string? linhvuc { get; set; }
        public string? mucdo { get; set; }
        public string? ten_thu_tuc { get; set; }
        public string? ma_thu_tuc { get; set; }
    }
}
