using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Property
    {
        
        public string PropertyNumber { get; set; }

        public string DerivativeNumber { get; set; }

        public string TakeNumber { get; set; }     
        
        public string MeterNumber { get; set; }        
        
        public string TypeService { get; set; }

        public string GeneralDataUserId { get; set; }

        public bool IsActive { get; set; } = true;        

        public bool IsOwner { get; set; } = false;

        public bool IsEnabled { get; set; } = false;
    }
}
