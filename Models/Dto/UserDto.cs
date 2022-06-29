using System.ComponentModel.DataAnnotations;

namespace API.Models.Dto
{
    public class UserDto
    {
        [Required(ErrorMessage = "Por favor teclee el campo Correo")]
        [StringLength(255)]
        [EmailAddress(ErrorMessage = "Por favor teclee el correcto formato para el campo Correo Ej: xxxxxx@zzzzz.dfd")]
        [Display(Name = "Correo")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Por favor teclee el campo Contraseña")]
        [StringLength(255)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Por favor teclee el campo Comprobar Contraseña")]
        [StringLength(255)]
        [Compare("Password", ErrorMessage = "La Contraseñas no Coinciden")]
        [DataType(DataType.Password)]
        [Display(Name = "Comprobar Contraseña")]
        public string RepeatPassword { get; set; }
    }
}
