using System.ComponentModel.DataAnnotations;

namespace API.Models.Dto
{
    public class PropertyDeleteDto
    {
        [Required(ErrorMessage = "Por favor teclee el campo Número de Inmueble")]
        [StringLength(16)]
        [Display(Name = "Inmueble")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Por favor teclee el campo Correo")]
        [StringLength(255)]
        [EmailAddress(ErrorMessage = "Por favor teclee el correcto formato para el campo Correo Ej: xxxxxx@zzzzz.dfd")]
        [Display(Name = "Correo")]
        public string Email { get; set; }
    }
}
