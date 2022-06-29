using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models.Dto
{
    public class CreditCardDto
    {

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

        [Required(ErrorMessage = "Por favor teclee el campo Correo")]
        [StringLength(255)]
        [EmailAddress(ErrorMessage = "Por favor teclee el correcto formato para el campo Correo Ej: xxxxxx@zzzzz.dfd")]
        [Display(Name = "Correo")]

        public string Email { get; set; }

        //[Display(Name = "Tarjeta Principal")]
        //public bool IsDefault { get; set; } = false;
    }
}
