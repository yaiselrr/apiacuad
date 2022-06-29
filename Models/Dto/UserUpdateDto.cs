using System.ComponentModel.DataAnnotations;

namespace API.Models.Dto
{
    public class UserUpdateDto
    {
        [Required(ErrorMessage = "Por favor teclee el campo Correo")]
        [StringLength(255)]
        [EmailAddress(ErrorMessage = "Por favor teclee el correcto formato para el campo Correo Ej: xxxxxx@zzzzz.dfd")]
        [Display(Name = "Correo")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Por favor teclee el campo Nombres")]
        [StringLength(40)]
        [Display(Name = "Nombres")]
        public string Names { get; set; }

        [Required(ErrorMessage = "Por favor teclee el campo 1er Apellido")]
        [StringLength(40)]
        [Display(Name = "1er Apellido")]
        public string FirstLastName { get; set; }

        [Required(ErrorMessage = "Por favor teclee el campo 2do Apellido")]
        [StringLength(40)]
        [Display(Name = "2do Apellido")]
        public string SecondLastName { get; set; }

        [Required(ErrorMessage = "Por favor teclee el campo 2do Título")]
        [StringLength(4)]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Phone(ErrorMessage = "Por favor teclee el formato correcto para el Celular")]
        [Range(0, 9999999999)]
        [Display(Name = "Celular")]
        public string MobileNumber { get; set; }

        [Phone(ErrorMessage = "Por favor teclee el formato correcto para el Teléfono Fijo")]
        [Range(0, 9999999999)]
        [Display(Name = "Teléfono Fijo")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Por favor teclee el campo Sexo")]
        [StringLength(8)]
        [Display(Name = "Sexo")]
        public string Sex { get; set; }

        [StringLength(255)]
        [Display(Name = "Dirección")]
        public string Address { get; set; }
    }
}
