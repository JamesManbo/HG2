using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HG.WebApp
{
    [Table("Language")]
    public class Language
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public bool IsDefault { get; set; }

      
    }
}
