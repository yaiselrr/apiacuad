using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class ConfirmEmail
    {

        [Required(ErrorMessage = "Por favor teclee el campo Id Usuario")]
        [StringLength(255)]
        [Display(Name = "Id Usuario")]

        public string UserId { get; set; }
    }
}
