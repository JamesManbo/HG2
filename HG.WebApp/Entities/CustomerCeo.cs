using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HG.WebApp
{
    [Table("CustomerCeo")]
    public class CustomerCeo
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Phone { get; set; }

        public string? FaxNumber { get; set; }
        public string? MobileNumber { get; set; }

        public string? TaxNumber { get; set; } 

        public string? Gmail { get; set; } 
        public string? Address { get; set; }

        public string? WebUri { get; set; }

        public string? CapitalNumber { get; set; }
        //Sosial
        public string? Facebook { get; set; }
        public string? Intagram { get; set; }
        public string? Tweit { get; set; }
        public string? Youtube { get; set; }
        public string? Linked { get; set; }
        public string? Whatapp { get; set; }

        //legalrepresentative
        public int AppUserID { get; set; }
        public int ListtingID { get; set; }

    }
}
