using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HG.WebApp.Dto { 
    public class UserDTO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Display(Name = "Tên")]
        public string? FirstName { get; set; }

        [Display(Name = "Họ")]
        public string? LastName { get; set; }

        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Tài khoản")]
        public string? UserName { get; set; }

        [Display(Name = "Email")]
        public string? Email { get; set; }
        public Guid? RoleId { get; set; }

        //public IList<string> Roles { get; set; }
    }
    
}