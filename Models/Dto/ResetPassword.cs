using System.ComponentModel.DataAnnotations;

namespace API.Models.Dto
{
    public class ResetPassword
    {
        [Required(ErrorMessage = "Por favor teclee el campo Id")]
        [StringLength(255)]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Por favor teclee el campo Contraseña")]
        [StringLength(255)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Por favor teclee el campo Confirmar Contraseña")]
        [StringLength(255)]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        public string ConfirmPassword { get; set; }
    }
}
