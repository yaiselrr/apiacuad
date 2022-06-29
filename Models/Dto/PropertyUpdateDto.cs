using System.ComponentModel.DataAnnotations;

namespace API.Models.Dto
{
    public class PropertyUpdateDto
    {
        [Required(ErrorMessage = "Por favor teclee el campo Inmueble")]
        public string PropertyNumber { get; set; }

        [Required(ErrorMessage = "Por favor teclee el campo Derivada")]
        public string DerivativeNumber { get; set; }

        [Required(ErrorMessage = "Por favor teclee el campo Toma")]
        public string TakeNumber { get; set; }

        public string MeterNumber { get; set; }

        public string TypeService { get; set; }

        [Required(ErrorMessage = "Por favor teclee el campo Correo")]
        [StringLength(255)]
        [EmailAddress(ErrorMessage = "Por favor teclee el correcto formato para el campo Correo Ej: xxxxxx@zzzzz.dfd")]
        [Display(Name = "Correo")]
        public string Email { get; set; }
    }
}
