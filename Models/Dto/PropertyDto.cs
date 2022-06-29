using System.ComponentModel.DataAnnotations;

namespace API.Models.Dto
{
    public class PropertyDto
    {
        [Required(ErrorMessage = "Por favor teclee el campo Correo")]
        [StringLength(255)]
        [EmailAddress(ErrorMessage = "Por favor teclee el correcto formato para el campo Correo Ej: xxxxxx@zzzzz.dfd")]
        [Display(Name = "Correo")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Por favor teclee el campo Inmueble")]
        [Display(Name = "Propiedad")]
        public int PropertyNumber { get; set; }

        [Required(ErrorMessage = "Por favor teclee el campo Derivada")]
        [Display(Name = "Derivada")]
        public int DerivativeNumber { get; set; }

        [Required(ErrorMessage = "Por favor teclee el campo Toma")]
        [Display(Name = "Toma")]
        public int TakeNumber { get; set; }
    }
}
