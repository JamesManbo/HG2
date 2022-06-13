using System.Collections.Generic;

namespace HG.WebApp.Models
{
    public class NotifyAjax
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public List<LoginRequest> LoginRequest { get; set; }
    }
}
