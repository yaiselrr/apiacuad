using System.ComponentModel.DataAnnotations;

namespace API.Models.Dto
{
    public class RoleDto
    {
        [Required(ErrorMessage = "Por favor teclee el campo Nombre de Rol")]
        [StringLength(16)]
        [Display(Name = "Rol")]
        public string RoleName { get; set; }
    }
}
