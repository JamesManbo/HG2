using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HG.WebApp
{
    [Table("Nation")]
    public class Nation
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public string? NameAbbreviation { get; set; }
        public string? Icon { get; set; }
        public int? Status { get; set; }
    }
}
