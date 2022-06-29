using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class GeneralDataUser : IdentityUser
    {
        
        [StringLength(40)]
        [Column(TypeName = "VARCHAR(40)")]
        public string Names { get; set; }
        
        [StringLength(40)]
        [Column(TypeName = "VARCHAR(40)")]
        public string FirstLastName { get; set; }
        
        [StringLength(40)]
        [Column(TypeName = "VARCHAR(40)")]
        public string SecondLastName { get; set; }
        
        [StringLength(10)]
        [Column(TypeName = "VARCHAR(10)")]
        public string Title { get; set; }
        
        [Phone(ErrorMessage = "Por favor teclee el formato correcto para el Celular")]
        [Range(0, 9999999999)]
        [Column(TypeName = "VARCHAR(10)")]
        public string MobileNumber { get; set; }
        
        [StringLength(9)]
        [Column(TypeName = "VARCHAR(9)")]
        public string Sex { get; set; }

        [StringLength(255)]
        [Column(TypeName = "VARCHAR(255)")]
        public string Address { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
