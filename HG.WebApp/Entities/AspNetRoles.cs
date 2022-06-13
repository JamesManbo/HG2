using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HG.WebApp
{

    [Table("AspNetRoles")]
    public class AspNetRoles : IdentityRole<Guid>
    {
        public string? Description { get; set; }
    }
}
