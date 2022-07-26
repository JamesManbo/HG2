using HG.Entities.Entities.SuperAdmin;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HG.Entities
{
    public class AspNetUsersModel 
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Password { get; set; }
    }
    public class UnitsMenuRolesOfUser
    {
        public Dm_Phong_Ban? dm_Phong_Ban { get; set; }
        public List<Nhom_Vai_Tro>? nhom_Vai_Tro { get; set; }
        public List<Asp_vaitro_quyen>? Vai_Tro_Menu_Quyen { get; set; }
    }
    public class Nhom_Vai_Tro
    {
        public string? ma_nhom { get; set; }
        public string? ma_vai_tro { get; set; }
    }
}
