using System.ComponentModel.DataAnnotations.Schema;

namespace HG.WebApp.Entities
{
    [Table("Locations")]
    public class Location
    {
        public int Id { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }
        public string? ZipCode { get; set; }
        public string? Country { get; set; } 
        public string? Latitude { get; set; } 
        public string? Longitude { get; set; }
        public int ListtingID { get; set; }

    }
}
