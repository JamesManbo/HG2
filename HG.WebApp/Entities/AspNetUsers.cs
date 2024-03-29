﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HG.WebApp
{
    [Table("AspNetUsers")]
    public class AspNetUsers : IdentityUser<Guid>
    {
        public string? ho_dem { get; set; }
        public string? ten { get; set; }
        public string? mat_khau { get; set; }
        public string? anh_dai_dien { get; set; }
        public string? anh_cmt { get; set; }
        public string? ho_khau_tt { get; set; }
        public string? PhoneNumber { get; set; }
        public string? tinh_trang_hon_nhan { get; set; }
        public Guid? RoleId { get; set; }
        public string? ma_phong_ban { get; set; }
        public string? ma_chuc_vu { get; set; }
        public int? stt { get; set; }
        public DateTime? ngay_sinh { get; set; }
        public int? Deleted { get; set; }
        public int? khoa_tai_khoan { get; set; }
        public int? Type { get; set; }
        public int? IsAdministratorPB { get; set; }
        public int? IsAdministratorDV { get; set; }
        public DateTime? CreatedDateUtc { get; set; }
        public Guid? CreatedUid { get; set; }
        public DateTime? UpdatedDateUtc { get; set; }
        public Guid? UpdatedUid { get; set; }
        public Guid? DeletedBy { get; set; }


    }
}
