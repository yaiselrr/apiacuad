using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models.Dto
{
    public class CreditCardUpdateDto
    {
        [Required(ErrorMessage = "Por favor teclee el campo Id")]
        [Range(0, 9999999999)]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Por favor teclee el campo número de Tarjeta de Crédito")]
        [StringLength(16)]
        [CreditCard]
        [Display(Name = "Número de Tarjeta")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Por favor teclee el campo Fecha de Expiración")]
        [RegularExpression(@"^((0[1-9]|1[0-2])/(201[4-9]|202[0-9]))$", ErrorMessage = "Characters are not allowed.")]
        [Display(Name = "Fecha de Expiración")]
        public string ExpiredDate { get; set; }

        [Required(ErrorMessage = "Por favor teclee el campo Nombre de Usuario")]
        [StringLength(100)]
        [Display(Name = "Nombre Propietario")]
        public string UserName { get; set; }

        [Display(Name = "Tarjeta Principal")]
        public bool IsDefault { get; set; } = false;
    }
}
