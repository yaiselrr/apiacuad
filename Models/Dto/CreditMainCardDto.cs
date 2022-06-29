using System.ComponentModel.DataAnnotations;

namespace API.Models.Dto
{
    public class CreditMainCardDto
    {
        [Required(ErrorMessage = "Por favor teclee el campo Id")]
        [Range(0, 9999999999)]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Por favor teclee el campo Correo")]
        [StringLength(255)]
        [EmailAddress(ErrorMessage = "Por favor teclee el correcto formato para el campo Correo Ej: xxxxxx@zzzzz.dfd")]
        [Display(Name = "Correo")]

        public string Email { get; set; }
    }
}
