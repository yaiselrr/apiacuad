using System.ComponentModel.DataAnnotations;

namespace API.Models.Dto
{
    public class JoinUserRoleDto
    {
        [Required(ErrorMessage = "Por favor teclee el campo Id Rol")]
        [StringLength(255)]
        [Display(Name = "Id Rol")]
        public string IdRole { get; set; }
        
        [Required(ErrorMessage = "Por favor teclee el campo Id Usuario")]
        [StringLength(255)]
        [Display(Name = "Id Usuario")]
        public string IdUser { get; set; }
    }
}
