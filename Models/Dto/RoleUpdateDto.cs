using System.ComponentModel.DataAnnotations;

namespace API.Models.Dto
{
    public class RoleUpdateDto
    {
        [Required(ErrorMessage = "Por favor teclee el campo Id")]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Por favor teclee el campo Nombre de Rol")]
        [StringLength(16)]
        [Display(Name = "Rol")]
        public string RoleName { get; set; }
    }
}
